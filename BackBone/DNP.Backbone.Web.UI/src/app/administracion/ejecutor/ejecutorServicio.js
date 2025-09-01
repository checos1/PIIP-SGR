(function () {
    'use strict';
    angular.module('backbone').factory('ejecutorServicio', ejecutorServicio);

    ejecutorServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'utilidades'];


    function ejecutorServicio($q, $http, $location, constantesBackbone, utilidades) {
        return {
            consultarEjecutor: consultarEjecutor,
            guardarEjecutor: guardarEjecutor,
        };

        function consultarEjecutor(nit) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarEjecutor;
            url += "?nit=" + nit;
            return $http.get(url);
        }

        function guardarEjecutor(objEjecutor) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarEjecutor;
            return $http.post(url, objEjecutor);
        }
    }
})();