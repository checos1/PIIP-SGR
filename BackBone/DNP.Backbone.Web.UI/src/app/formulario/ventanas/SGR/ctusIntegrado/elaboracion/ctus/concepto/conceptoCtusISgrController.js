(function () {
    'use strict';
    conceptoCtusISgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'ctusElaboracionSgrServicio'
    ];

    function conceptoCtusISgrController(  
        $scope,
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        ctusElaboracionSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctusintegradoselaboracionctusconcepto';

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.disabled = false;
        vm.activarConcepto = true;
        vm.desactivarConcepto = true;
        vm.ProyectoCtusId = null;

        vm.data;
        vm.form = {
            ProyectoCtusId: null,
            AspectosTecnicos: '',
            AspectosFinancieros: '',
            AspectosJuridicos: '',
            AspectosSociales: '',
            AspectosAmbientales: ''
        }

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

           ObtenerProyectoCtus(vm.proyectoId, vm.idInstancia);
            //var observador = transversalSgrServicio.registrarObservador(function (datos) {
            //    if (datos.proyectoCtus != undefined) {
            //        vm.ProyectoCtusId = datos.proyectoCtus.id;
            //        ObtenerProyectoCtuscConcepto(vm.ProyectoCtusId);
            //    }
            //});

            // Asegurarse de remover el observador cuando el controlador se destruye
            //$scope.$on('$destroy', observador);
        };

        function ObtenerProyectoCtus(proyectoId, idInstancia) {
            return ctusElaboracionSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, idInstancia).then(
                function (ProyectoCtus) {
                    vm.ProyectoCtus = ProyectoCtus.data;
                    vm.ProyectoCtusId = ProyectoCtus.data.id;

                    vm.disabled = $sessionStorage.soloLectura;

                    ctusElaboracionSgrServicio.SGR_CTUS_LeerProyectoCtusConcepto(vm.ProyectoCtusId)
                        .then(function (response) {
                            if (response.data != null) {
                                vm.form = response.data;
                                vm.formTemporal = { ...response.data };
                            }
                        }, function (error) {
                            utilidades.mensajeError('Ocurrió un problema al leer la información del concepto CTUS.');
                            return "";
                        });
                }
            );
        }


        function ObtenerProyectoCtuscConcepto(proyectoCtusId) {
            ctusElaboracionSgrServicio.SGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId)
                .then(function (response) {
                    if (response.data != null) {
                        vm.form = response.data;
                        vm.formTemporal = { ...response.data };
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer la información del concepto CTUS.');
                    return "";
                });
        }

        vm.ActivarEditar = function () {
            if (vm.activarConcepto) {
                $("#EditarConcepto").html("CANCELAR");
                vm.activarConcepto = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarConcepto").html("EDITAR");
                    vm.activarConcepto = true;
                    vm.form = { ...vm.formTemporal };
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
            if (vm.form.AspectosTecnicos === null || vm.form.AspectosTecnicos === undefined || vm.form.AspectosTecnicos === "") {
                utilidades.mensajeError('El campo Aspectos técnicos es obligatorio', false);
                return;
            }
            if (vm.form.AspectosFinancieros === "" || vm.form.AspectosFinancieros === null || vm.form.AspectosFinancieros === undefined) {
                utilidades.mensajeError('El campo Aspectos financieros es obligatorio', false);
                return;
            }
            if (vm.form.AspectosJuridicos == "" || vm.form.AspectosJuridicos == null || vm.form.AspectosJuridicos == undefined) {
                utilidades.mensajeError('El campo Aspectos jurídicos es obligatorio', false);
                return;
            }
            if (vm.form.AspectosSociales == "" || vm.form.AspectosSociales == null || vm.form.AspectosSociales == undefined) {
                utilidades.mensajeError('El campo Aspectos sociales es obligatorio', false);
                return;
            }
            if (vm.form.AspectosAmbientales == "" || vm.form.AspectosAmbientales == null || vm.form.AspectosAmbientales == undefined) {
                utilidades.mensajeError('El campo Aspectos ambientales es obligatorio', false);
                return;
            }

            vm.form.ProyectoCtusId = vm.ProyectoCtusId;

            Guardar();
        }

        function Guardar() {
            vm.form.ProyectoCtusId = vm.ProyectoCtusId;
            return ctusElaboracionSgrServicio.SGR_CTUS_GuardarProyectoCtusConcepto(vm.form).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                            $("#EditarConcepto").html("EDITAR");
                            vm.activarConcepto = true;
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
                            if (TipoError == 'SGRERRSEC') {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
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
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }
    }


    angular.module('backbone').component('conceptoCtusISgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/ctus/concepto/conceptoCtusISgr.html",
        controller: conceptoCtusISgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        });;
})();