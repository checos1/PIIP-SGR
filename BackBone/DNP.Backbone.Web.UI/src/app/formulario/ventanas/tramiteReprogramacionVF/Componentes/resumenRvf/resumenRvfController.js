(function () {
    'use strict';

    resumenRvfController.$inject = ['$scope', 'tramiteReprogramacionVfServicio', 'utilidades', '$sessionStorage'];

    function resumenRvfController(
        $scope,
        tramiteReprogramacionVfServicio,
        utilidades,
        $sessionStorage,
    ) {
        var vm = this;
        vm.nombreComponente = "resumenRvf";
        vm.notificacionCambiosCapitulos = null;
        vm.listaResumen = null;
        vm.Bandera = '-';
        vm.mostrarConstanteVig = false;
        vm.mostrarCorrienteVig = false;
        vm.mostrarConstanteProducto = false;
        vm.mostrarCorrienteProducto = false;
        //Totales Constantes Vigencia
        vm.sumAutorizadoNacionConstanteVig = 0;
        vm.sumAutorizadoPropiosConstanteVig = 0;
        vm.sumUtilizadoNacionConstanteVig = 0;
        vm.sumUtilizadoPropiosConstanteVig = 0;
        vm.sumReprogramadoNacionConstanteVig = 0;
        vm.sumReprogramadoPropiosConstanteVig = 0;
        vm.sumModificadoNacionConstanteVig = 0;
        vm.sumModificadoPropiosConstanteVig = 0;
        vm.totalAutorizadoNacionPropiosConstanteVig = 0;
        vm.totalUtilizadoNacionPropiosConstanteVig = 0;
        vm.totalReprogramadoNacionPropiosConstanteVig = 0;
        vm.totalModificadoNacionPropiosConstanteVig = 0;

        //Totales Corrientes Vigencia
        vm.sumAutorizadoNacionCorrienteVig = 0;
        vm.sumAutorizadoPropiosCorrienteVig = 0;
        vm.sumUtilizadoNacionCorrienteVig = 0;
        vm.sumUtilizadoPropiosCorrienteVig = 0;
        vm.sumReprogramadoNacionCorrienteVig = 0;
        vm.sumReprogramadoPropiosCorrienteVig = 0;
        vm.sumModificadoNacionCorrienteVig = 0;
        vm.sumModificadoPropiosCorrienteVig = 0;
        vm.totalAutorizadoNacionPropiosCorrienteVig = 0;
        vm.totalUtilizadoNacionPropiosCorrienteVig = 0;
        vm.totalReprogramadoNacionPropiosCorrienteVig = 0;
        vm.totalModificadoNacionPropiosCorrienteVig = 0;

        vm.init = function () {            
            //consultarResumenReprogramacionPorProductoVigencia();
        };

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                consultarResumenReprogramacionPorProductoVigencia();
            }
        });
        
        function consultarResumenReprogramacionPorProductoVigencia() {
            let instanciaId = $sessionStorage.idInstancia;
            let proyectoId = 0;
            let tramiteId = vm.tramiteid;
            vm.listaResumen = null;
            
            return tramiteReprogramacionVfServicio.obtenerResumenReprogramacionPorProductoVigencia(instanciaId, proyectoId, tramiteId)
                .then(respuesta => {                    
                    if (respuesta.data != "null") {
                        if (respuesta.data) {
                            vm.listaResumen = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                            vm.EsConstante = vm.listaResumen.EsConstante;
                            vm.mostrarTab(1);
                            vm.mostrarTabProducto(1);


                            //Totales Vigencias constantes
                            if (vm.listaResumen) {
                                if (vm.listaResumen.ResumenTramite) {
                                    if (vm.listaResumen.ResumenTramite[0].Valores) {
                                        let valoresConstantes = vm.listaResumen.ResumenTramite[0].Valores;

                                        valoresConstantes.forEach(item => {
                                            vm.sumAutorizadoNacionConstanteVig += item.AutorizadoNacion;
                                            vm.sumAutorizadoPropiosConstanteVig += item.AutorizadoPropios;
                                            vm.sumUtilizadoNacionConstanteVig += item.UtilizadoNacion;
                                            vm.sumUtilizadoPropiosConstanteVig += item.UtilizadoPropios;
                                            vm.sumReprogramadoNacionConstanteVig += item.ReprogramadoNacion;
                                            vm.sumReprogramadoPropiosConstanteVig += item.ReprogramadoPropios;
                                            vm.sumModificadoNacionConstanteVig += item.ModificadoNacion;
                                            vm.sumModificadoPropiosConstanteVig += item.ModificadoPropios;
                                        });

                                        vm.totalAutorizadoNacionPropiosConstanteVig = vm.sumAutorizadoNacionConstanteVig + vm.sumAutorizadoPropiosConstanteVig;
                                        vm.totalUtilizadoNacionPropiosConstanteVig = vm.sumUtilizadoNacionConstanteVig + vm.sumUtilizadoPropiosConstanteVig;
                                        vm.totalReprogramadoNacionPropiosConstanteVig = vm.sumReprogramadoNacionConstanteVig + vm.sumReprogramadoPropiosConstanteVig;
                                        vm.totalModificadoNacionPropiosConstanteVig = vm.sumModificadoNacionConstanteVig + vm.sumModificadoPropiosConstanteVig;
                                    }

                                    //Totales Vigencias corrientes
                                    if (vm.listaResumen.ResumenTramite[0].ValoresCorrientes) {
                                        let valoresCorrientes = vm.listaResumen.ResumenTramite[0].ValoresCorrientes;

                                        valoresCorrientes.forEach(item => {
                                            vm.sumAutorizadoNacionCorrienteVig += item.AutorizadoNacion;
                                            vm.sumAutorizadoPropiosCorrienteVig += item.AutorizadoPropios;
                                            vm.sumUtilizadoNacionCorrienteVig += item.UtilizadoNacion;
                                            vm.sumUtilizadoPropiosCorrienteVig += item.UtilizadoPropios;
                                            vm.sumReprogramadoNacionCorrienteVig += item.ReprogramadoNacion;
                                            vm.sumReprogramadoPropiosCorrienteVig += item.ReprogramadoPropios;
                                            vm.sumModificadoNacionCorrienteVig += item.ModificadoNacion;
                                            vm.sumModificadoPropiosCorrienteVig += item.ModificadoPropios;
                                        });

                                        vm.totalAutorizadoNacionPropiosCorrienteVig = vm.sumAutorizadoNacionCorrienteVig + vm.sumAutorizadoPropiosCorrienteVig;
                                        vm.totalUtilizadoNacionPropiosCorrienteVig = vm.sumUtilizadoNacionCorrienteVig + vm.sumUtilizadoPropiosCorrienteVig;
                                        vm.totalReprogramadoNacionPropiosCorrienteVig = vm.sumReprogramadoNacionCorrienteVig + vm.sumReprogramadoPropiosCorrienteVig;
                                        vm.totalModificadoNacionPropiosCorrienteVig = vm.sumModificadoNacionCorrienteVig + vm.sumModificadoPropiosCorrienteVig;
                                    }
                                }
                            }
                        }
                    }
                    else {
                        vm.listaResumen = "";
                    }
                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la tabla resumen de reprogramación");
                });
        }

        vm.AbrilNivel = function (objeto) {
            var variable = $("#ico" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#ico" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#ico" + objeto).attr("src", "Img/btnMas.svg");
            }
        }

        vm.mostrarTab = function (origen) {
            vm.TipoValor = origen;
            switch (origen) {
                case 1:
                    {
                        //Constantes
                        vm.mostrarConstanteVig = true;
                        vm.mostrarCorrienteVig = false;
                        break;
                    }
                case 2:
                    {
                        //Corrientes
                        vm.mostrarCorrienteVig = true;
                        vm.mostrarConstanteVig = false;
                        break;
                    }
            }
        }

        vm.mostrarTabProducto = function (origen) {
            vm.TipoValorProducto = origen;
            switch (origen) {
                case 1:
                    {
                        //Constantes
                        vm.mostrarConstanteProducto = true;
                        vm.mostrarCorrienteProducto = false;
                        break;
                    }
                case 2:
                    {
                        //Corrientes
                        vm.mostrarCorrienteProducto = true;
                        vm.mostrarConstanteProducto = false;
                        break;
                    }
            }
        }

        vm.verMas = function (e) {
            var ATTRIBUTES = ['titlevalue', 'textvalue'];

            var $target = $(e.target);
            var modalSelector = $target.data('target');

            ATTRIBUTES.forEach(function (attributeName) {
                var $modalAttribute = $(modalSelector + ' #modal-' + attributeName);
                var dataValue = $target.data(attributeName);
                $modalAttribute.text(dataValue || '');
            });
        }
    }

    angular.module('backbone').component('resumenRvf', {
        templateUrl: "src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/resumenRvf/resumenRvf.html",

        controller: resumenRvfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            tramiteid: '@',
        },
    });

})();