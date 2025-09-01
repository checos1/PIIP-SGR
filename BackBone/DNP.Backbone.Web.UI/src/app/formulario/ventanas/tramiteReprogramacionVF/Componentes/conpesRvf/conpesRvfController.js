(function () {
    'use strict';

    conpesRvfController.$inject = [
        '$scope',
        '$filter',
        'conpesServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'constantesBackbone',
        '$sessionStorage',
    ];

    function conpesRvfController(
        $scope,
        $filter,
        conpesServicio,
        utilidades,
        justificacionCambiosServicio,
        constantesBackbone,
        $sessionStorage,
    ) {
        var vm = this;
        vm.lang = "es";

        vm.clearConpesIndex = 0;

        vm.conpesPreseleccionados = [];
        vm.nombreComponente = "informacionpresupuestalconpes";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.init = function () {            
            vm.setEvents();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        }

        vm.setEvents = function () {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '') {
                    vm.getConpesPreseleccionados()
                }
            });
        }

        vm.getConpesPreseleccionados = function () {
            conpesServicio.ObtenerConpesTramites(vm.tramiteid)
                .then(function (response) {
                    if (response.data.Estado) {
                        vm.conpesPreseleccionados = [...response.data.Data];
                    }
                }, function (error) {
                    utilidades.mensajeError('No fue posible consultar el listado de conpes');
                });
        }

        vm.guardarCompes = function (compesList) {
            
            if (vm.tramiteid === null || vm.tramiteid === undefined || vm.tramiteid === '') {
                utilidades.mensajeError('Debe seleccionar al menos un CONPES para asociar');
                return;
            }
            if (compesList === undefined || compesList.length === 0) {
                utilidades.mensajeError('Debe seleccionar al menos un CONPES para asociar');
                return;
            }

            const model = {
                tramiteId: vm.tramiteid,
                Conpes: compesList
            }

            conpesServicio.asociarCompesVigenciaFutura(model)
                .then(function (response) {
                    if (response?.data?.Estado) {
                        vm.getConpesPreseleccionados();
                        utilidades.mensajeSuccess($filter('language')('MensajeExitoConpesTramiteVF'),
                            false,
                            () => { },
                            null,
                            $filter('language')('MensajeExitoDatosGuardados')

                        );
                        //para guardar los capitulos modificados y que se llenen las lunas
                        guardarCapituloModificado();
                        vm.clearConpesIndex = vm.clearConpesIndex + 1;
                    } else {
                        vm.paginationModel.hasError = true;
                        utilidades.mensajeError('No fue posible consultar el listado de conpes');
                    }
                }, function (error) {
                    utilidades.mensajeError('No fue posible consultar el listado de conpes');
                });
        }

        vm.removerConpes = function (conpes) {
            if (conpes === undefined || conpes === null) {
                return;
            }

            const model = {
                tramiteId: vm.tramiteid,
                id: conpes.id,
                NumeroConpes: conpes.numeroCONPES
            }

            utilidades.mensajeWarning(
                $filter('language')('ValidacionBorradoConpesTramiteVFCuerpo'),
                function () {
                    vm.deleteConpesRequest(model);
                },
                function () { },
                null,
                null,
                $filter('language')('ValidacionBorradoConpesTramiteVF')
            );

        }

        vm.deleteConpesRequest = function (model) {
            conpesServicio.removerAsociacionConpesTramiteVigenciaFutura(model)
                .then(function (response) {
                    if (response?.data?.Estado) {
                        vm.getConpesPreseleccionados();
                        utilidades.mensajeSuccess($filter('language')('MensajeExitoBorradoConpesTramiteVF'),
                            false,
                            () => { },
                            null
                        );
                        //para guardar los capitulos modificados y que se llenen las lunas
                        if (vm.conpesPreseleccionados.length - 1 == 0) {
                            eliminarCapitulosModificados();

                        }
                    } else {
                        vm.paginationModel.hasError = true;
                        utilidades.mensajeError('No fue posible remover el conpes seleccionado');
                    }
                }, function (error) {
                    utilidades.mensajeError('No fue posible remover el conpes seleccionado');
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
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

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
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
            const span = document.getElementById('id-capitulo-informacionpresupuestalconpes');
            vm.seccionCapitulo = span.textContent;


        }

        /* ------------------------ Validaciones ---------------------------------*/
        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                else {
                    var isValid = true;
                }

                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-conpes-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionPresupuestalConpes = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-conpes-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'IFO003': vm.validarValoresVigenciaInformacionPresupuestalConpes
        }
        /* ------------------------ FIN Validaciones ---------------------------------*/
    }


    angular.module('backbone').component('conpesRvf', {
        templateUrl: "src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/conpesRvf/conpesRvf.html",
        controller: conpesRvfController,
        controllerAs: "vm",
        bindings: {
            tramiteid: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            callback: '&',

        }
    });

})();