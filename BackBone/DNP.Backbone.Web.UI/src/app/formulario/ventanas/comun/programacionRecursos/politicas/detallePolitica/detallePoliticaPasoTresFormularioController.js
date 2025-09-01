var detallePoliticaPasoTresFormularioCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('detallePoliticaPasoTresFormulario', detallePoliticaPasoTresFormulario);
    detallePoliticaPasoTresFormulario.$inject = [
        '$scope',
        '$uibModal',
        '$log',
        '$q',
        '$sessionStorage',
        '$localStorage',
        '$timeout',
        '$location',
        '$filter',
        'comunesServicio',
        'utilidades'
    ];

    function detallePoliticaPasoTresFormulario(
        $scope,
        $uibModal,
        $log,
        $q,
        $sessionStorage,
        $localStorage,
        $timeout,
        $location,
        $filter,
        comunesServicio,
        utilidades) {
        var vm = this;
        detallePoliticaPasoTresFormularioCtrl = vm;
        //variables
        vm.lang = "es";
        vm.TabActivo = 1;
        vm.initDetalle = initDetalle;
        vm.mostrarTabConcepto = false;
        vm.obtenerPoliticasTransversales = this.obtenerPoliticasTransversales;

        vm.ActivarTab = function (tab) {
            vm.TabActivo = tab;
        }

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoid) {
                    obtenerPoliticasTransversales();
                }
            }
        });

        vm.notificarRefrescoPoliticasTransversales = null;
        vm.notificarRefrescoPoliticasCruce = null;
        vm.notificarRefrescoPoliticasConcepto = null;
        vm.notificarRefrescoPoliticas = null;

        vm.guardado = function (nombreComponenteHijo) {
            //vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });

            if (nombreComponenteHijo == "politicastransversalespoliticastransv") {
                vm.notificarRefrescoPoliticasTransversales();
                vm.notificarRefrescoPoliticasCruce();
                vm.notificarRefrescoPoliticasConcepto();
                obtenerPoliticasTransversales();
            } else if (nombreComponenteHijo == "focalizacionpolresumendeformul") {
                vm.notificarRefrescoPoliticasCruce();
            } else if (nombreComponenteHijo == "conceptoVictimasPasoTresFormulario") {
                vm.notificarRefrescoPoliticas();
            }
        }

        vm.notificarRefresco = function (handler, nombreComponente) {

            if (nombreComponente == "focalizacionpolresumendeformul") {
                vm.notificarRefrescoPoliticasTransversales = handler;
            }
            else if (nombreComponente == "conceptoVictimasPasoTresFormulario") {
                vm.notificarRefrescoPoliticasConcepto = handler;
            }
            else if (nombreComponente == "crucepoliticastransversalesprogramacion") {
                vm.notificarRefrescoPoliticasCruce = handler;
            } else if (nombreComponente == "politicastransversalespoliticastransv") {
                vm.notificarRefrescoPoliticas = handler;
            }
        };

        function initDetalle() { }

        function obtenerPoliticasTransversales() {
            vm.mostrarTabConcepto = false;
            return comunesServicio.consultarPoliticasTransversalesProgramacion(vm.tramiteproyectoid).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        let arreglolistas = jQuery.parseJSON(respuesta.data);
                        let arregloGeneral = jQuery.parseJSON(arreglolistas);
                        let arregloDatosGenerales = arregloGeneral.Politicas;
                        let listaPoliticasProy = [];

                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                TieneConceptoPendiente: arregloDatosGenerales[pl].TieneConceptoPendiente
                            }

                            if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento)
                                listaPoliticasProy.push(politicasProyecto);
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);

                        let politicaVictimas = vm.listaPoliticasProyectos.filter(x => x.politicaId == 20); //política víctimas
                        if (politicaVictimas.length > 0) {
                            vm.mostrarTabConcepto = true;
                        }
                    }
                });
        }
    }
    angular.module('backbone')
        .component('detallePoliticaPasoTresFormulario', {
            templateUrl: 'src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/detallePoliticaPasoTresFormulario.html',
            controller: 'detallePoliticaPasoTresFormulario',
            controllerAs: 'vm',
            bindings: {
                tramiteproyectoid: '@',
                proyectoid: '@',
                origen: '@',
                codigobpin: '@',
                guardadoevent: '&',
                notificacioncambios: '&',
                notificacionvalidacion: '&',
                notificacionestado: '&',
                notificarrefresco: '&',
                callback: '&',
                actualizacomponentes: '@',
                calendarioiniciativas: '@',
                modificardistribucion: '=',
                notificaciontramiteproyecto: '&',
                calendariopoliticastransversales: '@'
            }
        });
})();