(function () {
    'use strict';
    angular.module('backbone').factory('indicadorPoliticasServicio', indicadorPoliticasServicio);

    indicadorPoliticasServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function indicadorPoliticasServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerIndicadores: obtenerIndicadores,
            Guardar: Guardar,
            obtenerCalendarioPeriodo: obtenerCalendarioPeriodo
        };

        function obtenerIndicadores(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerIndicadores, model);
        }

        function obtenerCalendarioPeriodo(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerCalendarioPeriodo, model);
        }

        function Guardar(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriIndicadorPoliticaSeguimientoPeriodosValores, model);
        }
    }
})();