var detallePoliticaFormularioCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('detallePoliticaFormulario', detallePoliticaFormulario);
    detallePoliticaFormulario.$inject = [
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

    function detallePoliticaFormulario(
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
        detallePoliticaFormularioCtrl = vm;
        //variables
        vm.lang = "es";
        vm.TabActivo = 1;
        vm.initDetalle = initDetalle;
        vm.mostrarTabConcepto = false;
        vm.mostrarTabConceptoValores = true;
        vm.obtenerPoliticasTransversales = this.obtenerPoliticasTransversales;

        vm.ActivarTab = function (tab) {
            vm.TabActivo = tab;
        }

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoid) {
                    obtenerPoliticasTransversales();
                    obtenerPoliticasModificaciones();
                }
            }
        });

        $scope.$watch('vm.calendariopoliticastransversales', function () {

            if (vm.calendariopoliticastransversales != '') {
                console.log("vm.calendariopoliticastransversales -> " + vm.calendariopoliticastransversales);
            }
        });

        vm.notificarRefrescoPoliticasTransversales = null;
        vm.notificarRefrescoPoliticasCruce = null;
        vm.notificarRefrescoPoliticasConcepto = null;
        vm.notificarRefrescoPoliticas = null;
        vm.notificarRefrescoConceptoValores = null;

        vm.guardado = function (nombreComponenteHijo) {
            //vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });

            if (nombreComponenteHijo == "politicasTransversalesMlFormulario") {
                vm.notificarRefrescoPoliticasTransversales();
                //vm.notificarRefrescoPoliticasCruce();
                //vm.notificarRefrescoPoliticasConcepto();
                obtenerPoliticasTransversales();
            } else if (nombreComponenteHijo == "resumenFocalizacionMlFormulario") {
                vm.notificarRefrescoConceptoValores();
                //vm.notificarRefrescoPoliticasCruce();
                obtenerPoliticasModificaciones();
            } else if (nombreComponenteHijo == "conceptoVictimasMlFormulario") {
                vm.notificarRefrescoPoliticas();
            } else if (nombreComponenteHijo == "conceptoValoresMlFormulario") {
                vm.notificarRefrescoConceptoValores();
            }
            vm.callback();
        }

        vm.notificarRefresco = function (handler, nombreComponente) {

            if (nombreComponente == "resumenFocalizacionMlFormulario") {
                vm.notificarRefrescoPoliticasTransversales = handler;
            }
            else if (nombreComponente == "conceptoVictimasMlFormulario") {
                vm.notificarRefrescoPoliticasConcepto = handler;
            }
            else if (nombreComponente == "crucepoliticastransversalesprogramacion") {
                vm.notificarRefrescoPoliticasCruce = handler;
            } else if (nombreComponente == "politicasTransversalesMlFormulario") {
                vm.notificarRefrescoPoliticas = handler;
            }
            else if (nombreComponente == "conceptoValoresMlFormulario") {
                vm.notificarRefrescoConceptoValores = handler;
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
                                TieneConceptoPendiente: arregloDatosGenerales[pl].TieneConceptoPendiente,
                                Mensaje: arregloDatosGenerales[pl].Mensaje,
                                EnFirme: arregloDatosGenerales[pl].EnFirme
                            };

                            if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento)
                                listaPoliticasProy.push(politicasProyecto);
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);

                        let politicaVictimas = vm.listaPoliticasProyectos.filter(x => x.politicaId == 20); //política víctimas
                        if (politicaVictimas.length > 0 ) {
                            if ( politicaVictimas[0].EnFirme == 0 && politicaVictimas[0].enSeguimiento == 0) {
                                vm.mostrarTabConcepto = true;
                            }                            
                        }
                    }
                });
        }

        function obtenerPoliticasModificaciones() {

            vm.mostrarTabConceptoValores = false;
            return comunesServicio.consultarPoliticasTransversalesAprobacionesModificaciones(vm.tramiteproyectoid, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolista = jQuery.parseJSON(respuesta.data);
                        vm.lstPoliticasModifica = jQuery.parseJSON(arreglolista);
                        if (vm.lstPoliticasModifica) {
                            if (vm.lstPoliticasModifica.Politicas) {
                                if (vm.lstPoliticasModifica.Politicas.length > 0) {
                                    vm.mostrarTabConceptoValores = true;
                                }
                            }
                        }
                    }
                }
            );
        }
    }

    angular.module('backbone')
        .component('detallePoliticaFormulario', {
            templateUrl: '/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/detallePoliticaFormulario.html',
            controller: 'detallePoliticaFormulario',
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
                calendariopoliticastransversales: '@',
                mostrarconcepto: '@'
            }
        });
})();