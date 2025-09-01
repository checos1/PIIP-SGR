(function () {
    'use strict';
    angular.module('backbone').factory('relacionPlanificacionServicio', relacionPlanificacionServicio);

    relacionPlanificacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

  
    function relacionPlanificacionServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerDocumentosConpes: obtenerDocumentosConpes,
            obtenerCambiosFirme: obtenerCambiosFirme
        };

        function obtenerDocumentosConpes(idProyecto, instanciaId, guiMacroproceso, NivelId, FlujoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramiteCargarProyectoConpes;
            url += "?proyectoid=" + idProyecto;
            url += "&InstanciaId=" + instanciaId;
            url += "&GuiMacroproceso=" + guiMacroproceso;
            url += "&NivelId=" + NivelId;
            url += "&FlujoId=" + FlujoId;
            return $http.get(url);
        }

        function obtenerCambiosFirme(idProyecto, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCambiosFirmeRelacionPlanificacion;
            url += "?idProyecto=" + idProyecto;
            return $http.get(url);
        }       
    }
})();