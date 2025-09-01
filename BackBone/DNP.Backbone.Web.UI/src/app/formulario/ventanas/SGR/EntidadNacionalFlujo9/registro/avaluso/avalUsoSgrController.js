(function () {
    'use strict';
    
    avalUsoSgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'avalusoViabilidadSgrServicio',
        'transversalSgrServicio',
        'justificacionCambiosServicio'
    ];

    function avalUsoSgrController(
        $scope,
        utilidades,
        $sessionStorage,
        avalusoViabilidadSgrServicio,
        transversalSgrServicio,
        justificacionCambiosServicio
    ) {

        var vm = this;
        vm.respuestaAval = undefined; // Variable para almacenar la respuesta del usuario
        vm.preguntaAvalUsoSgr = '';
        vm.lang = "es";
        vm.nombreComponente = "sgrviabilidadavalusoavaluso";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        vm.paramertros = {
            ProyectoId: vm.proyectoId,
            InstanciaId: vm.idInstancia,
            cumpleAvalUso: false
        };


        vm.disabled = $sessionStorage.soloLectura;
        vm.activar = true;
        vm.desactivar = true;

        vm.init = function () {
            leerDatosAvalUsoSgr();
            leerParametroPreguntaAvalUsoSgr();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.notificarRefresco, nombreComponente: vm.nombreComponente });
        };

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarG").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarG").html("EDITAR");
                    vm.activar = true;
                    vm.init();

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.guardar = function () {
            Guardar();
        }

        function leerDatosAvalUsoSgr() {
            avalusoViabilidadSgrServicio.SGR_Proyectos_LeerAvalUsoSgr(vm.proyectoId, vm.idInstancia)
                .then(function (response) {
                    var respuestaJson = JSON.parse(response.data);
                    vm.respuestaAval = respuestaJson ? (respuestaJson.cumpleAvalUso ? 1 : 0) : undefined;
                })
                .catch(function (error) {
                    if (error.status === 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    } else {
                        utilidades.mensajeError("Error al realizar la operación");
                    }
                });
        }

        function leerParametroPreguntaAvalUsoSgr() {
            transversalSgrServicio.SGR_Transversal_LeerParametro("PreguntaAvalUsoSgr")
                .then(function (respuestaParametro) {
                    if (respuestaParametro !== undefined) {
                        if (respuestaParametro.data) {
                            vm.preguntaAvalUsoSgr = respuestaParametro.data.Valor || '¿Requiere insumo de otro sector para la emisión del concepto?';
                        }
                    } else {
                        vm.preguntaAvalUsoSgr = '¿Requiere insumo de otro sector para la emisión del concepto?';
                    }
                });
        }

        function Guardar() {

            if (vm.respuestaAval === undefined) {
                utilidades.mensajeError("Debe seleccionar una opción para la pregunta");
                return;
            }

            vm.paramertros = {
                ProyectoId: vm.proyectoId,
                InstanciaId: vm.idInstancia,
                cumpleAvalUso: (vm.respuestaAval === 1 || vm.respuestaAval === '1') ? true : false
            };

            return avalusoViabilidadSgrServicio.SGR_Proyectos_RegistrarAvalUso(vm.paramertros).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        
                        var resultado = JSON.parse(response.data);
                        if (resultado.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            $sessionStorage.soloLectura = false;
                            utilidades.mensajeSuccess("", false, false, false);

                            $("#EditarG").html("EDITAR");
                            vm.activar = true;
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        vm.guardadohijos = function (nombreComponenteHijo) {
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
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
                        if (TipoError == 'SGRAVAL1') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        } else {
                            vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
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
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != null) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }
            var campomensajeerror = document.getElementById('SGRAVAL1sgrviabilidadavalusoavaluso');
            campomensajeerror.innerHTML = "";
            campomensajeerror.classList.add('hidden');
        }

        /* ------------------------ Final Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('avalUsoSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/registro/avaluso/avalUsoSgr.html",
        controller: avalUsoSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificarrefresco: '&',
            guardadocomponent: '&',
        },
    });
})();



