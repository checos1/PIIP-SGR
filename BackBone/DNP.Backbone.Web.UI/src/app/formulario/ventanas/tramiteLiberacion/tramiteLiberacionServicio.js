(function () {
    'use strict';
    angular.module('backbone').factory('tramiteLiberacionServicio', tramiteLiberacionServicio);

    tramiteLiberacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function tramiteLiberacionServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            obtenerDatosProyectoTramite: obtenerDatosProyectoTramite,
            ObtenerTramitesVFparaLiberar: ObtenerTramitesVFparaLiberar,
            GuardarLiberacionVigenciaFutura: GuardarLiberacionVigenciaFutura,
            obtenerTarmitesPorProyectoEntidad: obtenerTarmitesPorProyectoEntidad,
            EliminarLiberacionVigenciaFutura: EliminarLiberacionVigenciaFutura
        };

        function obtenerDatosProyectoTramite(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProyectoTramite;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function ObtenerTramitesVFparaLiberar(numTramite) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramitesVFparaLiberar;
            url += "?numtramite=" + numTramite;
            return $http.get(url);
        }

        function GuardarLiberacionVigenciaFutura(liberacionDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarLiberacionVigenciaFutura;
            return $http.post(url, liberacionDto);
        }

        function obtenerTarmitesPorProyectoEntidad(entidadId, proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriobtenerTarmitesPorProyectoEntidad;
            url += "?proyectoId=" + proyectoId + "&entidadId=" + entidadId;
            return $http.get(url);
        }

        function EliminarLiberacionVigenciaFutura(tramiteEliminarDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarLiberacionVigenciaFutura;
            return $http.post(url, tramiteEliminarDto);
        }
}
}) ();