(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('servicioCreditos', servicioCreditos);

    servicioCreditos.$inject = ['$http', 'constantesBackbone', '$location'];

    function servicioCreditos($http, constantesBackbone, $location) {

        return {
            obtenerContraCreditos: obtenerContraCreditos,
            obtenerCreditos: obtenerCreditos,
            guardarProyectos: guardarProyectos,
           
            obtenerJsonLocal: obtenerJsonLocal,
        }
                
        function obtenerContraCreditos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerContraCreditos;
            return $http.post(url, parametros);
        }     
        function obtenerCreditos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCredito;
            return $http.post(url, parametros);
        }

        function guardarProyectos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarProyectos;
            return $http.post(url, parametros);
        }     

        //--------p/ mock------------//
        function obtenerJsonLocal(nombreJson) {
            var url = 'http://localhost:3024/src/assets/' + nombreJson + '.json';

            return $http({
                method: 'GET',
                'Content-Type': 'application/json;charset=utf-8',
                url: url
            });
        }
    }
})();
