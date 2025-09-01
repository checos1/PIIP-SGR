(function () {
    'use strict';
    registroAprobacionSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'aprobacionSgrServicio',
        'justificacionCambiosServicio',
    ];

    function registroAprobacionSgrController(
        utilidades,
        $sessionStorage,
        aprobacionSgrServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgraprobacion1aprobacionregistroaprobacion';
        vm.nombreComponenteResumen = "sgraprobacion1aprobacionresumenaprobacion";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.seccionCapituloEstado = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.fechahoy = new Date();

        vm.ConvertirNumero = ConvertirNumero;
        vm.ObtenerNumero = ObtenerNumero;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true;
        vm.AddVigencia = false;

        vm.data;

        vm.AprobacionProyectoDetalle = {

        }

        vm.init = function () {
            ObtenerProyectoAprobacionInstanciasSGR(vm.idInstancia);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
        }

        function ObtenerProyectoAprobacionInstanciasSGR(idInstancia) {
            return aprobacionSgrServicio.ObtenerProyectoAprobacionInstanciasSGR(idInstancia).then(
                function (AprobacionProyectoDetalle) {
                    vm.AprobacionProyectoDetalle = AprobacionProyectoDetalle.data[0];
                    vm.AprobacionProyectoDetalle.FechaAprobacion = vm.AprobacionProyectoDetalle.FechaAprobacion ? new Date(vm.AprobacionProyectoDetalle.FechaAprobacion) : null;
                    vm.AprobacionProyectoDetalle.FechaActoAdmtvo = vm.AprobacionProyectoDetalle.FechaActoAdmtvo ? new Date(vm.AprobacionProyectoDetalle.FechaActoAdmtvo) : null;

                }
            );
        }

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
                    ObtenerProyectoAprobacionInstanciasSGR(vm.idInstancia);
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

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ObtenerNumero(obj) {
            return typeof obj == 'string' ? parseFloat(obj.replace(",", ".")) : obj;
        }

        vm.Guardar = function () {
            if (!validar()) {
                return;
            }
            else {
                if (vm.AprobacionProyectoDetalle.Aprobado == 'true' || vm.AprobacionProyectoDetalle.Aprobado == true) {
                    utilidades.mensajeWarning("La aprobación no podrá ser modificada ni eliminada. ¿Está seguro de continuar?", function funcionContinuar() {
                        Guardar();
                    }, function funcionCancelar(reason) {
                        return;
                    }, null, null, `El proyecto quedara marcado como aprobado.`);
                }
                else {
                    Guardar();
                }
            }

        }

        function validar() {
            var valida = true;
            var PreguntaObligatoria = document.getElementById('PreguntaObligatoria');
            var PreguntaObligatoria1 = document.getElementById('PreguntaObligatoria1');
            //var PreguntaObligatoria2 = document.getElementById('PreguntaObligatoria2');

            if (vm.AprobacionProyectoDetalle.Aprobado === null || vm.AprobacionProyectoDetalle.Aprobado === undefined || vm.AprobacionProyectoDetalle.Aprobado === "") {
                if (PreguntaObligatoria != undefined) {
                    PreguntaObligatoria.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                if (PreguntaObligatoria != undefined) {
                    PreguntaObligatoria.classList.add('hidden');
                }
            }

            if (vm.AprobacionProyectoDetalle.FechaAprobacion === null || vm.AprobacionProyectoDetalle.FechaAprobacion === undefined || vm.AprobacionProyectoDetalle.FechaAprobacion === "") {
                if (PreguntaObligatoria1 != undefined) {
                    PreguntaObligatoria1.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                if (PreguntaObligatoria1 != undefined) {
                    PreguntaObligatoria1.classList.add('hidden');
                }
            }

            //if (vm.AprobacionProyectoDetalle.FechaActoAdmtvo === null || vm.AprobacionProyectoDetalle.FechaActoAdmtvo === undefined || vm.AprobacionProyectoDetalle.FechaActoAdmtvo === "") {
            //    if (PreguntaObligatoria2 != undefined) {
            //        PreguntaObligatoria2.classList.remove('hidden');
            //    }
            //    valida = false;
            //}
            //else {
            //    if (PreguntaObligatoria2 != undefined) {
            //        PreguntaObligatoria2.classList.add('hidden');
            //    }
            //}

            return valida;
        }

        function Guardar() {
            
             return aprobacionSgrServicio.GuardarProyectoAprobacionInstanciasSGR(vm.AprobacionProyectoDetalle).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        //guardarCapituloModificadoResumen();
                        utilidades.mensajeSuccess("", false, false, false);

                        $("#EditarG").html("EDITAR");
                        vm.activar = true;
                        vm.limpiarErrores();
                        ObtenerPriorizionProyectoDetalle(vm.idInstancia);
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
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

        function guardarCapituloModificadoResumen() {
            ObtenerSeccionCapituloResumen();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapituloEstado,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponenteResumen });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }
        function ObtenerSeccionCapituloResumen() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponenteResumen);
            vm.seccionCapituloEstado = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            vm.validarSeccion(TipoError,p.Descripcion, false);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid }); 

            }
        }

        vm.validarSeccion = function (tipoError, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError);
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
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }
    }


    angular.module('backbone').component('registroAprobacionSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/registro/registroAprobacionSgr.html",
        controller: registroAprobacionSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })       
})();