(function () {
    'use strict';

    angular.module('backbone').factory('transversalSgrServicio', transversalSgrServicio);

    transversalSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function transversalSgrServicio($http, constantesBackbone) {

        return {
            SGR_Transversal_LeerParametro: SGR_Transversal_LeerParametro,
            SGR_Transversal_ObtenerConfiguracionReportes: SGR_Transversal_ObtenerConfiguracionReportes,
            registrarObservador: registrarObservador,
            removerObservador: removerObservador,
            notificarCambio: notificarCambio
        };

        function SGR_Transversal_LeerParametro(parametro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSGR_Transversal_LeerParametro + "?parametro=" + parametro;
            return $http.get(url);
        }

        function SGR_Transversal_ObtenerConfiguracionReportes(instanciaId) {
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

            // Devolver una función de eliminación
            return () => removerObservador(callback);
        }

        function removerObservador(callback) {
            const indice = observadores.indexOf(callback);
            if (indice !== -1) {
                observadores.splice(indice, 1);
            }
        }

        function notificarCambio(datos) {
            if (observadores) {
                observadores.forEach(callback => callback(datos));
            }
        }
        /*Fin - Usado para comunicar los componentes hijos*/
    }
})();