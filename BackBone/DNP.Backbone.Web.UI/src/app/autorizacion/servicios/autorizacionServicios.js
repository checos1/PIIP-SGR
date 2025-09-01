(function (apiAutorizacionServicioBaseUri, nombreAplicacionBackbone) {
    'use strict';

    angular.module('backbone').factory('autorizacionServicios', autorizacionServicios);

    autorizacionServicios.$inject = ['$http', 'utilidades', 'sesionServicios', 'constantesAutorizacion', 'constantesBackbone'];

    function autorizacionServicios($http, utilidades, sesionServicios, constantesAutorizacion, constantesBackbone) {

        return {
            obteneryPersistirRoles: obteneryPersistirRoles,
            obtenerPermisosPorEntidad: obtenerPermisosPorEntidad,
            obtenerRolesPorUsuario: obtenerRolesPorUsuario,
            obtenerEntidadesPorRoles: obtenerEntidadesPorRoles,
            obtenerTiposEntidad: obtenerTiposEntidad,
            obtenerListaEntidad: obtenerListaEntidad,
            cambiarModulo: cambiarModulo,
            obtenerOpcionesconfiltro: obtenerOpcionesconfiltro,
        }

        ////////////

        function obteneryPersistirRoles(idUsuarioDNP) {
            return this.obtenerRolesPorUsuario(idUsuarioDNP).then(
                function (roles) {
                    sesionServicios.setearUsuarioRoles(roles);
                }
            );
        }

        function obtenerPermisosPorEntidad() {
            var url = apiBackboneServicioBaseUri + 'api/Usuario/ObtenerPermisosPorEntidad?noblockui';
            return $http.get(url)
                .then(utilidades.httpRequestComplete);
        }

        function obtenerRolesPorUsuario(idUsuarioDNP) {
            var url = apiBackboneServicioBaseUri + 'api/Usuario/ObtenerRolesPorUsuario?noblockui';
            var cuerpo = {};

            return $http.post(url, cuerpo)
                .then(utilidades.httpRequestComplete);
        }

        function obtenerEntidadesPorRoles(idsRoles) {

            var url = apiBackboneServicioBaseUri + 'api/Usuario/ObtenerEntidadesPorRoles';

            if (!idsRoles) {
                idsRoles = sesionServicios.obtenerUsuarioIdsRoles();
            }

            var configuracion = {
                params: {
                    idAplicacionDnp: nombreAplicacionBackbone,
                    IdsRoles: idsRoles
                }
            }
            return $http.get(url, configuracion)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestErrorReject);
        }

        function obtenerTiposEntidad() {
            return [
                {
                    clave: constantesAutorizacion.tipoEntidadNacional,
                    nombre: constantesAutorizacion.tipoEntidadNacional
                },
                {
                    clave: constantesAutorizacion.tipoEntidadTerritorial,
                    nombre: constantesAutorizacion.tipoEntidadTerritorial
                },
                {
                    clave: constantesAutorizacion.tipoEntidadSGR,
                    nombre: constantesAutorizacion.tipoEntidadSGR
                },
                {
                    clave: constantesAutorizacion.tipoEntidadPrivadas,
                    nombre: constantesAutorizacion.tipoEntidadPrivadas
                },
                {
                    clave: constantesAutorizacion.tipoEntidadPublicas,
                    nombre: 'Públicas'
                }
            ];
        }

        function obtenerListaEntidad(idUsuarioDNP, objetoNegocioId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiAutorizacionObtenerListaEntidad;

            if (objetoNegocioId !== null && objetoNegocioId !== undefined) {
                var config = {
                params: {
                        usuarioDnp: idUsuarioDNP,
                        objetoNegocioId: objetoNegocioId
                    }
                }
            }
            else {
                var config = {
                    params: {
                        usuarioDnp: idUsuarioDNP
                    }
                }
            }

            return $http.get(url, config)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestErrorReject);
        }

        function cambiarModulo() {
            const tenant = esUsuarioB2C && "b2c" || "b2b";
            window.open(`${backboneURL}Account/SwitchSignShared?redirectUrl=${administracionURL}&tenant=${tenant}`, '_blank');
        }

        function obtenerOpcionesconfiltro(idAp) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiAutorizacionObtenerOpcionesConFiltro}`;
            return $http.get(url + "?idAplicacion=" + idAp)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestErrorReject);
        }

       
    }

})(apiAutorizacionServicioBaseUri, nombreAplicacionBackbone);