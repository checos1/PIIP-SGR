(function () {
    'use strict';

    angular.module('backbone').factory('transversalSgpServicio', transversalSgpServicio);

    transversalSgpServicio.$inject = ['$http', 'constantesBackbone'];

    function transversalSgpServicio($http, constantesBackbone) {

        return {
            SGPTransversalLeerParametro: SGPTransversalLeerParametro,
            SGP_Transversal_ObtenerConfiguracionReportes: SGP_Transversal_ObtenerConfiguracionReportes,
            registrarObservador: registrarObservador,
            limpiarObservadores: limpiarObservadores,
            notificarCambio: notificarCambio
        };

        function SGPTransversalLeerParametro(parametro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSGPTransversalLeerParametro + "?parametro=" + parametro;
            return $http.get(url);
        }

        function SGP_Transversal_ObtenerConfiguracionReportes(instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSGR_Transversal_ObtenerConfiguracionReportes + "?instanciaId=" + instanciaId;
            return $http.get(url);
        }

        /*Inicio - Usado para comunicar los componentes hijos*/
        var observadores = [];

        function registrarObservador(callback) {
            if (!observadores) {
                observadores = [];
            }
            observadores.push(callback);
        }

        function notificarCambio(datos) {
            observadores.forEach(function (observador) {
                observador(datos);
            });
        }

        function limpiarObservadores() {
            observadores = [];
        }
        /*Fin - Usado para comunicar los componentes hijos*/
    }
})();