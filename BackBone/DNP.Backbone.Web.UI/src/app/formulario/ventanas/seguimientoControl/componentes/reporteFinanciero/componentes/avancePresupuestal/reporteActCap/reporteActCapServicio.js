(function () {
    'use strict';
    angular.module('backbone').factory('reporteActCapServicio', reporteActCapServicio);

    reporteActCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function reporteActCapServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListadoObjProdNiveles: obtenerListadoObjProdNiveles,
            obtenerCalendarioPeriodo: obtenerCalendarioPeriodo
        };

        function obtenerListadoObjProdNiveles(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerListadoObjProdNivelesReporte, model);
        }

        function obtenerCalendarioPeriodo(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerCalendarioPeriodo, model);
        }
    }
})();