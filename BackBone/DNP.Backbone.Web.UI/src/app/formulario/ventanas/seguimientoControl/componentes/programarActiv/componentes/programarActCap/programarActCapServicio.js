(function () {
    'use strict';
    angular.module('backbone').factory('programarActCapServicio', programarActCapServicio);

    programarActCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function programarActCapServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListadoObjProdNiveles: obtenerListadoObjProdNiveles
        };

        function obtenerListadoObjProdNiveles(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerListadoObjProdNivelesProgramar, model);
        }
    }
})();