(function () {
    'use strict';
    sepOcadPazSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'viabilidadSgrServicio',
        'justificacionCambiosServicio',
    ];

    function sepOcadPazSgrController(
        utilidades,
        $sessionStorage,
        viabilidadSgrServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sistemaevaluacionpuntajesistemaevaluacionpuntaje";
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idEntidad = $sessionStorage.idEntidad;

        vm.disabled = $sessionStorage.soloLectura;
        vm.activar = true;

        vm.PuntajesProyecto = {};

        vm.init = function () {
            ObtenerPuntajeProyecto();
        };

        vm.mostrarBotonPdf = function () {
            var mostrar = false;
            if (vm.nombreComponente == "sistemaevaluacionpuntajesistemaevaluacionpuntaje") { mostrar = true; }
            return mostrar;
            ;
        };

        vm.verPdf = function () {
        }

        function ObtenerPuntajeProyecto()
        {
            viabilidadSgrServicio.SGR_ObtenerPuntajeProyecto(vm.idInstancia, vm.idEntidad).then(function (response) {
                if (response.data) {
                    vm.PuntajesProyecto = JSON.parse(response.data);
                    vm.PuntajesProyecto.InstanciaId = vm.idInstancia;
                }
            }).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        vm.mostrarNivel2 = function (idNivel1, idNivel2) {
            var nivel1 = vm.PuntajesProyecto.Variables.find((e, i) => e.VbleId == idNivel1);
            var nivel2 = nivel1.Variables.find((e, i) => e.VbleId == idNivel2);
            if (typeof (nivel2.Variables) !== 'undefined' && nivel2.Variables !== null && nivel2.Variables.length > 0) {
                return true;
            } else {
                return false;
            }
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarReq").html("CANCELAR");
                vm.activar = false;
                vm.AddVigencia = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarReq").html("EDITAR");
                    vm.activar = true;
                    vm.AddVigencia = true;
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.guardar = function () {
            if (!validar()) {
                return;
            }
            else {
                Guardar();
            }
        }

        function validar() {
            const completo = (json, f) => {
                const traverse = (node) => {
                    if (!f(node)) return false;
                    if (node.Variables) {
                        return node.Variables.every(traverse);
                    }
                    return true;
                };
                return traverse(json);
            };

            var resultado = completo(vm.PuntajesProyecto, evaluar);

            if (!resultado) {
                if (MensajeFaltaDiligenciar != undefined) {
                    MensajeFaltaDiligenciar.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                if (MensajeFaltaDiligenciar != undefined && !MensajeFaltaDiligenciar.classList.contains("hidden")) {
                    MensajeFaltaDiligenciar.classList.add('hidden');
                }
            }

            return resultado;
        }

        function evaluar(nodo) {
            var resultado = true;
            if (typeof (nodo.MostrarOpcionesResultado) !== 'undefined' && nodo.MostrarOpcionesResultado != null && nodo.MostrarOpcionesResultado) {
                if (typeof (nodo.Resultado) === 'undefined' || nodo.Resultado == null || nodo.Resultado === '') {
                    resultado = false;
                }
            }
            return resultado;
        }

        function Guardar() {
                                         
            return viabilidadSgrServicio.guardarPuntajeProyecto(vm.PuntajesProyecto).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado(vm.nombreComponente);
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        $("#EditarReq").html("EDITAR");
                        vm.activar = true;
                        vm.AddVigencia = true;
                        vm.limpiarErrores();

                        ObtenerPuntajeProyecto();
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }


        function guardarCapituloModificado(nombreComponente) {
            ObtenerSeccionCapitulo(nombreComponente);
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo(nombreComponente) {
            const span = document.getElementById('id-capitulo-' + nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (TipoError == 'ErrorG') {
                            vm.validarValoresPasoAnterior(p.Descripcion);
                        } else if (TipoError == 'SGRERRSEC') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValoresPasoAnterior = function (errores) {
            var campoObligatorioJustificacion = document.getElementById('ErrorG');
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = errores;
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var errorValoresPasoAnterior = document.getElementById('ErrorG');
            errorValoresPasoAnterior.classList.add('hidden');

            var errorElements = document.getElementsByClassName('errorSeccionViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }
    }

    angular.module('backbone').component('sepOcadPazSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ocadPaz/viabilidad/viabilidad/sep/sepOcadPazSgr.html",
        controller: sepOcadPazSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();