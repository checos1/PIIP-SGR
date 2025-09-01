(function () {
    'use strict';
    angular.module('backbone').factory('justificacionCambiosSinTramiteSgpServicio', justificacionCambiosSinTramiteSgpServicio);

    justificacionCambiosSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];
   
    function justificacionCambiosSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerCapitulosModificados: obtenerCapitulosModificados,
            getIdEtapa: getIdEtapa,
            guardarCambiosFirme: guardarCambiosFirme,
            ObtenerSeccionCapitulo: ObtenerSeccionCapitulo,
            eliminarCapitulosModificados: eliminarCapitulosModificados,
            FocalizacionActualizaPoliticasModificadas: FocalizacionActualizaPoliticasModificadas,
        };

        function obtenerCapitulosModificados(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneCapitulosModificados;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function guardarCambiosFirme(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCambiosFirmeRelacionPlanificacion;
            return $http.post(url, data);
        }

        function getIdEtapa(etapa) {
            var idEtapa = [];
            switch (etapa) {
                case 'pl':
                    idEtapa = constantesBackbone.idEtapaPlaneacion;
                    break;
                case 'pr':
                    idEtapa = constantesBackbone.idEtapaProgramacion;
                    break;
                case 'gr':
                    idEtapa = constantesBackbone.idEtapaGestionRecursos;
                    break;
                case 'ej':
                    idEtapa = constantesBackbone.idEtapaNuevaEjecucion;
                    break;
                case 'se':
                    idEtapa = [];
                    break;
                case 'ev':
                    idEtapa = constantesBackbone.idEtapaEvaluacion;
                    break;
            }
            return idEtapa;
        }

        function ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, idUsuario, NivelId,FlujoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerSeccionCapitulo;
            url += "?FaseGuid=" + FaseGuid;
            url += "&Capitulo=" + Capitulo;
            url += "&Seccion=" + Seccion;
            url += "&idUsuario=" + idUsuario;
            url += "&NivelId=" + NivelId;
            url += "&FlujoId=" + FlujoId;
            return $http.get(url);
        }

        function eliminarCapitulosModificados(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonEliminarCambiosFirme;
            return $http.post(url, data);
        }

        function FocalizacionActualizaPoliticasModificadas(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFocalizacionActualizaPoliticasModificadas;
            return $http.post(url, data);
        }
    }
})();