
(function () {

    'use strict';

    angular.module('backbone').factory('backboneServiciosNegocio', backboneServiciosNegocio);

    backboneServiciosNegocio.$inject = ['$http', 'appSettings', 'constantesBackbone'];

    function backboneServiciosNegocio($http, appSettings,constantesBackbone) {
        var servicios = {
            obtenerProyectosEntidadesPorUsuarioRoles: obtenerProyectosEntidadesPorUsuarioRoles
        }
        return servicios;


        function obtenerProyectosEntidadesPorUsuarioRoles(usuarioDnp) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosEntidadesPorUsuario + usuarioDnp);
        }

        function obtenerProyectosEntidadesPorUsuarioRoles(usuarioDnp) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosEntidadesPorUsuario + usuarioDnp);
        }      

    }

})();