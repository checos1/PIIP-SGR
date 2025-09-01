(function () {
    'use strict';

    resumenFuentesCostosSgrController.$inject = ['$scope', 'previosSgrServicio', 'utilidades', '$sessionStorage', 'justificacionCambiosServicio'];

    function resumenFuentesCostosSgrController(
        $scope,
        previosSgrServicio,
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.nombreComponente = "sgrviabilidadpreviosrecursosresumencostos";
        vm.disabled = false;
        vm.listaFuentesEtapa = null;
        vm.etapaId = null;
        vm.nombreEtapa = null;
        vm.Bpin = null;
        vm.TotalCostosEtapa = 0;
        vm.TotalValorSolicitadoEtapa = 0;
        vm.erroresComponente = [];
        vm.componentesRefresh = [
            "sgrviabilidadpreviosrecursosfuentessgr",
            "sgrviabilidadpreviosrecursosfuentesnosgr"
        ];

        vm.init = function () {
           
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });

        };
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente) {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                consultarResumenFuentesCostos();
            }
        }

        function consultarResumenFuentesCostos() {

            var flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

            if (flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";
            }
            else {
                vm.Bpin = $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio;
            }

            let instanciaId = "";
            return previosSgrServicio.consultarResumenFuentesCostos(vm.Bpin, instanciaId)
                .then(respuesta => {
                    if (respuesta.data) {
                        vm.listaFuentes = angular.copy(jQuery.parseJSON(jQuery.parseJSON(respuesta.data)));
                    }

                    //Obtener los totales de costos y valor solicitado por etapa
                    if (vm.listaFuentes) {
                        if (vm.listaFuentes.Etapas) {
                            vm.TotalCostosEtapa = 0;
                            vm.TotalValorSolicitadoEtapa = 0;
                            vm.listaFuentes.Etapas.forEach(etapa => {
                                vm.TotalCostosEtapa += parseFloat(etapa.CostoEtapa);
                                vm.TotalValorSolicitadoEtapa += parseFloat(etapa.ValorSolicitadoEtapa);
                            });
                            if (vm.etapaId != null)
                                vm.mostrarTab(vm.etapaId);
                        }
                    }
                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la lista de proyectos");
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();

            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        vm.mostrarTab = function (origen) {
            vm.etapaId = origen;
            vm.listaFuentesEtapa = null;

            switch (origen) {
                case 1:
                    vm.nombreEtapa = "Preinversión";
                    if (vm.listaFuentes) {
                        vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                            return a.EtapaId === origen;
                        });
                    }
                    break;
                case 2:
                    vm.nombreEtapa = "Inversión";
                    if (vm.listaFuentes) {
                        vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                            return a.EtapaId === origen;
                        });
                    }
                    break;
                case 3:
                    vm.nombreEtapa = "Operación";
                    if (vm.listaFuentes) {
                        vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                            return a.EtapaId === origen;
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                consultarResumenFuentesCostos();
                guardarCapituloModificado();
            }
        }
    }

    angular.module('backbone').component('resumenFuentesCostosSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/resumenFuentesCostos/resumenFuentesCostosSgr.html",

        controller: resumenFuentesCostosSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            notificacioninicio: '&',
            guardadocomponent: '&',
        },
    });

})();