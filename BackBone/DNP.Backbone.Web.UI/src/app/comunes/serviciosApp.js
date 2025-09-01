(function () {

    'use strict';

    angular.module('backbone')
        .factory('backboneServicios', backboneServicios);

    backboneServicios.$inject = ['$http', '$localStorage', '$q', 'sesionServicios', 'utilidades', 'constantesBackbone', '$sessionStorage'];

    function backboneServicios($http, $localStorage, $q, sesionServicios, utilidades, constantesBackbone, $sessionStorage) {

        var servicios = {
            obtenerToken: obtenerToken,
            obtenerTokenChangePassword: obtenerTokenChangePassword,
            estaAutorizado: estaAutorizado,
            consultarPermiso: consultarPermiso,
            obtenerPermisosPorEntidad: obtenerPermisosPorEntidad,
            obtenerListaParametros: obtenerListaParametros
        }

        return servicios;

        ///////////////////////
        function obtenerToken() {
            return $http.post('/Account/ObtenerTokenAutorizacion');
        }

        function obtenerTokenChangePassword() {
            return $http.get('/Home/GetTokenChangePassword')
                .then(function (results) {
                    return results.data;
                },
                    function () {
                        location.reload();
                    });
        }

        function estaAutorizado() {
            return $localStorage.authorizationData ? true : false;
        }

        //********************//
        function consultarPermiso(entidad, opcion) {
            let tieneOpcion = false;
            try {
                const permisos = sesionServicios.obtenerPermisos()

                let entidadFiltro = permisos.Entidades.find(item => item.IdEntidad.toUpperCase() == entidad.toUpperCase());

                if (entidadFiltro) {
                    tieneOpcion = entidadFiltro.Opciones.some(item => item.IdOpcionDNP == opcion);
                }
            } catch (error) {
                throw `Parámetros no informados correctamente: ${error}`
            }
            return tieneOpcion;
        }

        function obtenerPermisosPorEntidad() {
            var url = apiBackboneServicioBaseUri + 'api/Usuario/ObtenerPermisosPorEntidad';
            return $http.get(url)
            .then(utilidades.httpRequestComplete);  
        }

        function obtenerListaParametros() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBone_LeerListaParametros;
            return $http.get(url);
        }
    }
})();