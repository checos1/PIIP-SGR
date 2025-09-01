(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.entidades').factory('servicioCargaDatos', servicioCargaDatos);

    servicioCargaDatos.$inject = ['$http', 'constantesBackbone', '$location'];

    function servicioCargaDatos($http, constantesBackbone, $location) {

        return {
            obtenerCargaDatos: obtenerCargaDatos,
            guardarDatos: guardarDatos,
            eliminarCargaDatos: eliminarCargaDatos,
            obtenerDatosMongoDb: obtenerDatosMongoDb,
            obtenerEntidadesPorTipo: obtenerEntidadesPorTipo,

            obtenerJsonLocal: obtenerJsonLocal,
        }

        function obtenerEntidadesPorTipo(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadPorTipoEntidad + tipoEntidad;
            return $http.get(url);
        }    

        function obtenerCargaDatos(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCargaDatos + tipoEntidad;
            return $http.get(url);
        }     

        function obtenerDatosMongoDb(id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosMongoDb + id;
            return $http.get(url);
        }     
        

        function guardarDatos(datos) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarDatos;
            return $http.post(url, datos, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } });
        }
        
        function eliminarCargaDatos(id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarCargaDatos+"/"+ id;
            return $http.post(url);
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
