(function () {
    'use strict';

    angular.module('backbone').factory('ctusIntegradoElaboracionSgrServicio', ctusIntegradoElaboracionSgrServicio);

    ctusIntegradoElaboracionSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function ctusIntegradoElaboracionSgrServicio($http, constantesBackbone) {

        return {
            ConsultarAccionPorInstancia: ConsultarAccionPorInstancia,
            DevolverCuestionarioProyecto: DevolverCuestionarioProyecto,
            obtenerErroresviabilidadSgr: obtenerErroresviabilidadSgr,
            SGR_CTUS_LeerProyectoCtusConcepto: SGR_CTUS_LeerProyectoCtusConcepto,
            SGR_CTUS_GuardarProyectoCtusConcepto: SGR_CTUS_GuardarProyectoCtusConcepto,
            SGR_Viabilidad_ValidarCargueDocumentoObligatorio: SGR_Viabilidad_ValidarCargueDocumentoObligatorio,
            SGR_Viabilidad_obtenerConceptosPreviosEmitidos: SGR_Viabilidad_obtenerConceptosPreviosEmitidos,
            SGR_CTUS_GuardarResultadoConceptoCtus: SGR_CTUS_GuardarResultadoConceptoCtus
        };

        function obtenerErroresviabilidadSgr(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function ConsultarAccionPorInstancia(instanciaId, idAccion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneConsultarAccionPorInstancia}?instanciaId=${instanciaId}&idAccion=${idAccion}`;
            return $http.get(url);
        }

        function DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneDevolverCuestionarioProyecto}?nivelId=${nivelId}&instanciaId=${instanciaId}&estadoAccionesPorInstancia=${estadoAccionesPorInstancia}`;
            return $http.post(url);
        }

        function SGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_CTUS_LeerProyectoCtusConcepto}?proyectoCtusId=${proyectoCtusId}`;
            return $http.get(url);
        }

        function SGR_CTUS_GuardarProyectoCtusConcepto(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_CTUS_GuardarProyectoCtusConcepto, data);
        }

        function SGR_Viabilidad_ValidarCargueDocumentoObligatorio(data, coleccion) {
            return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarArchivosSgr}${coleccion}`, data);
        }

        function SGR_Viabilidad_obtenerConceptosPreviosEmitidos(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptosPreviosEmitidos + "?bpin=" + Bpin;
            return $http.get(url);
        }

        function SGR_CTUS_GuardarResultadoConceptoCtus(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_CTUS_GuardarResultadoConceptoCtus, data);
        }
    }
})();