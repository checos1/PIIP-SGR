(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('viabilidadServicio', viabilidadServicio);

    viabilidadServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function viabilidadServicio($q, $http, $location, constantesBackbone) {

        return {
            obtenerPreguntasPersonalizadas: obtenerPreguntasPersonalizadas,
            obtenerDatosGeneralesProyecto: obtenerDatosGeneralesProyecto,
            obtenerConfiguracionEntidades: obtenerConfiguracionEntidades,
            guardarRespuestas: guardarRespuestas,
            crearLogFlujo: crearLogFlujo,
            obtener: obtener,
            devolverProyecto: devolverProyecto,
            ConsultarAccionPorInstancia: ConsultarAccionPorInstancia,
            DevolverCuestionarioProyecto: DevolverCuestionarioProyecto,
            obtenerEstadoInstancia: obtenerEstadoInstancia,
            obtenerErroresViabilidad: obtenerErroresViabilidad,
            ObtenerAmpliarDevolucionTramite: ObtenerAmpliarDevolucionTramite
        };

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadas;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

        function obtenerDatosGeneralesProyecto(ProyectoId, NivelId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosGeneralesProyecto;
            var params = {
                'ProyectoId': ProyectoId,
                'NivelId': NivelId,
            };
            return $http.get(url, { params });
        }

        function obtenerConfiguracionEntidades(ProyectoId, NivelId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConfiguracionEntidades;
            var params = {
                'ProyectoId': ProyectoId,
                'NivelId': NivelId,
            };
            return $http.get(url, { params });
        }

        function obtenerErroresViabilidad(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresViabilidad;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&IdProyecto=" + idProyecto;
            url += "&IdNivel=" + idNivel;
            url += "&IdInstancia=" + idInstancia;
            return $http.get(url);
        }

        function guardarRespuestas(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadas, parametros);
        }

        function crearLogFlujo(logs) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCrearLogFlujo, logs);
        }

        function obtener(instanciaId, nivelId) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerFlujosLogInstancia}?instanciaId=${instanciaId}&nivelId=${nivelId}`;
            return $http.get(url);
        }

        function devolverProyecto(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDevolverProyecto, parametros);
        }

        function ConsultarAccionPorInstancia(instanciaId, idAccion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneConsultarAccionPorInstancia}?instanciaId=${instanciaId}&idAccion=${idAccion}`;
            return $http.get(url);
        }

        function DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneDevolverCuestionarioProyecto}?nivelId=${nivelId}&instanciaId=${instanciaId}&estadoAccionesPorInstancia=${estadoAccionesPorInstancia}`;
            return $http.post(url);
        }

        function obtenerEstadoInstancia(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEstadoInstancia, peticion);
        }

        function ObtenerAmpliarDevolucionTramite(proyectoId, tramiteId) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerAmpliarDevolucionTramite}?ProyectoId=${proyectoId}&TramiteId=${tramiteId}`;
            return $http.get(url);
        }
    }
})();
