(function () {
    'use strict';

    angular.module('backbone').factory('servicioAcciones', servicioAcciones);

    servicioAcciones.$inject = ['$http', 'constantesBackbone'];

    function servicioAcciones($http, constantesBackbone) {

        return {
            ObtenerFlujoPorInstanciaTarea: obtenerFlujoPorInstanciaTarea,
            ObtenerAccionesDevolucion: obtenerAccionesDevolucion,
            DevolverFlujo: devolverFlujo,
            ObtenerTokenMGA: obtenerTokenMGA,
            obtenerValidacionVerAccion: obtenerValidacionVerAccion,
            obtenerInstanciaProyecto: obtenerInstanciaProyecto,
            obtenerPermisosAccionPaso: obtenerPermisosAccionPaso,
            obtenerEstadoOcultarObservacionesGenerales: obtenerEstadoOcultarObservacionesGenerales,
            validacionDevolucionPaso: validacionDevolucionPaso
        };

        function obtenerFlujoPorInstanciaTarea(parametros) {
                var url = apiBackboneServicioBaseUri + constantesBackbone.apiFlujosObtenerFlujoPorInstanciaTarea;
            return $http.get(url,
                {
                    params:
                    {
                        nombreAplicacion: parametros.nombreAplicacion,
                        usuarioDnp: parametros.usuario,
                        idInstancia: parametros.idInstancia
                    }
                });
        }

        function obtenerAccionesDevolucion(idInstancia, idAccion) {
            var urimetodo = constantesBackbone.apiAccionesDevolucion + idInstancia + "&idAccion=" + idAccion;
            return $http.get(apiBackboneServicioBaseUri + urimetodo);
        }

        function validacionDevolucionPaso(instanciaId, accionId, accionDevolucionId) {
            var urimetodo = constantesBackbone.apiBackboneValidacionDevolucionPaso + "?instanciaId=" + instanciaId + "&accionId=" + accionId + "&accionDevolucionId=" + accionDevolucionId;
            return $http.get(apiBackboneServicioBaseUri + urimetodo);
        }

        function devolverFlujo(parametrosDevolverFlujo) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiDevolverFlujo ,parametrosDevolverFlujo);
        }

        function obtenerTokenMGA(parametros, tipoUsuario) {
            return $http.get(`${apiBackboneServicioBaseUri}${constantesBackbone.apiObtenerTokenMGA}?bpin=${parametros}&tipoUsuario=${tipoUsuario}`);
        }

        function obtenerValidacionVerAccion(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosObtenerValidacionVerAccion, parametros);
        }

        function obtenerInstanciaProyecto(idInstancia, bpin) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiInstanciaProyecto + idInstancia + "&bpin=" + bpin);
        }

        function CrearTrazaAccionesPorInstancia( parametros) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosCrearTrazaAccionesPorInstancia, parametrosEjecucionFlujo);
        }

        function ObtenerDevolucionesPorIdInstanciaYIdAccion(idInstancia, idAccion) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiAccionesDevolucion + idInstancia + "&bpin=" + bpin);
        }

        function obtenerPermisosAccionPaso(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosObtenerPermisosAccionPaso, parametros);
        }

        function obtenerEstadoOcultarObservacionesGenerales() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosObtenerEstadoOcultarObservacionesGenerales);
        }

     }
})();
