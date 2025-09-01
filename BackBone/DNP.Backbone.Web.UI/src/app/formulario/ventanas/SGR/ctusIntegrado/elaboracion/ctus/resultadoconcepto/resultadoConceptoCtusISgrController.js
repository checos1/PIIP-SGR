(function () {
    'use strict';
    resultadoConceptoCtusISgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'ctusElaboracionSgrServicio'
    ];

    function resultadoConceptoCtusISgrController(
        $scope,
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        ctusElaboracionSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctusintegradoselaboracionctusresultadoconceptointegrado';

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.disabled = false;
        vm.activarResultadoConcepto = true;
        vm.desactivarResultadoConcepto = true;
        vm.ProyectoCtusId = null;
        vm.resultadoConcepto = null;


        vm.data;

        vm.ProyectoCtus = {
            id: 0,
            Concepto: '',
        }

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
            ObtenerProyectoCtus(vm.proyectoId, vm.idInstancia);

            //handle SendProyectoCtus event
            $scope.$on("SendProyectoCtus", function (evt, data) {
                vm.ProyectoCtusId = data.id;
                vm.resultadoConcepto = data.Concepto;
            });

            //handle isFormularioIncompletoRequisitosCtus event
            $scope.$on("isFormularioIncompletoRequisitosCtus", function (evt, data) {
                vm.isFormularioIncompletoRequisitosCtus = data;

                var seccionNoHabilitadaCtus = document.getElementById('seccionNoHabilitadaCtus');
                var seccionHabilitadaCtus = document.getElementById('seccionHabilitadaCtus');

                if (vm.isFormularioIncompletoRequisitosCtus) {
                    vm.disabledResultadoConcepto = true;

                    if (seccionNoHabilitadaCtus != undefined) {
                        seccionNoHabilitadaCtus.classList.remove('hidden');
                    }

                    if (seccionHabilitadaCtus != undefined) {
                        seccionHabilitadaCtus.classList.add('hidden');
                    }
                } else {
                    vm.disabledResultadoConcepto = false;

                    if (seccionHabilitadaCtus != undefined) {
                        seccionHabilitadaCtus.classList.remove('hidden');
                    }

                    if (seccionNoHabilitadaCtus != undefined) {
                        seccionNoHabilitadaCtus.classList.add('hidden');
                    }
                }
            });

            //handle isCumpleRequisitosCtus event
            $scope.$on("isCumpleRequisitosCtus", function (evt, data) {
                vm.isCumpleRequisitosCtus = data;
            });
        };

        function ObtenerProyectoCtus(proyectoId, idInstancia) {
            return ctusElaboracionSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, idInstancia).then(
                function (ProyectoCtus) {
                    vm.ProyectoCtus = ProyectoCtus.data;
                    vm.ProyectoCtusId = ProyectoCtus.data.id;
                    vm.resultadoConcepto = ProyectoCtus.data.Concepto;
                }
            );
        }

        vm.ActivarEditar = function () {
            if (vm.activarResultadoConcepto) {
                $("#EditarResultadoConcepto").html("CANCELAR");
                vm.activarResultadoConcepto = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarResultadoConcepto").html("EDITAR");
                    vm.activarResultadoConcepto = true;
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

            return ctusElaboracionSgrServicio.SGR_Proyectos_CumplimentoFlujoSGR(vm.idInstancia).then(
                function (ProyectoCtus) {
                    vm.isCumpleRequisitosCtus = ProyectoCtus.data;
                    vm.isCumpleRequisitosCtusBool = vm.isCumpleRequisitosCtus == 1 ? true : false;

                    if (vm.isCumpleRequisitosCtus === -1) {
                        swal('', "No se ha diligenciado los cuestionarios de requisitos de verificación. Debe diligenciarlos antes de continuar.", 'error');
                        return;
                    }

                    if (vm.isCumpleRequisitosCtusBool !== JSON.parse(vm.resultadoConcepto)) {
                        utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                            Guardar();
                            return;
                        }, null, null, null, "El resultado diligenciado no es consistente con el resultado de los requisitos de verificación");
                        return;
                    }

                    Guardar();
                }
            );
        }

        function validar() {
            var valida = true;
            var PreguntaObligatoria = document.getElementById('PreguntaObligatoria');

            if (vm.resultadoConcepto === null || vm.resultadoConcepto === undefined || vm.resultadoConcepto === "") {
                if (PreguntaObligatoria != undefined) {
                    PreguntaObligatoria.classList.remove('hidden');
                }
                valida = false;
            }
            else if (PreguntaObligatoria != undefined) {
                PreguntaObligatoria.classList.add('hidden');
            }

            return valida;
        }

        function Guardar() {

            vm.form = {
                ProyectoCtusId: vm.ProyectoCtusId,
                ResultadoConcepto: vm.resultadoConcepto
            }

            return ctusElaboracionSgrServicio.SGR_CTUS_GuardarResultadoConceptoCtus(vm.form).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("", false, false, false);
                            vm.limpiarErrores();

                            $("#EditarResultadoConcepto").html("EDITAR");
                            vm.activarResultadoConcepto = true;
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

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

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
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
                            if (TipoError == 'SGRCTUSConcepto') {
                                vm.validarErrores(TipoError, p.Descripcion, false);
                            }
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarErrores = function (tipoError, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove('ico-advertencia');
                }
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var campo = document.getElementById('SGRCTUSConcepto');
            if (campo != undefined) {
                campo.innerHTML = "";
                campo.classList.add('hidden');
            }
        }
    }


    angular.module('backbone').component('resultadoConceptoCtusISgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/ctus/resultadoconcepto/resultadoConceptoCtusISgr.html",
        controller: resultadoConceptoCtusISgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    });
})();