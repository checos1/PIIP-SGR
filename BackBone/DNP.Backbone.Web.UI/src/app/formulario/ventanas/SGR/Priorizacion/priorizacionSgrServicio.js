(function () {
    'use strict';

    angular.module('backbone').factory('priorizacionSgrServicio', priorizacionSgrServicio);

    priorizacionSgrServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function priorizacionSgrServicio($q, $http, $location, constantesBackbone) {
        return {
            consultarPriorizacionPorBPIN: consultarPriorizacionPorBPIN,
            SGR_Proyectos_MostrarEstadosPriorizacion: SGR_Proyectos_MostrarEstadosPriorizacion,
            obtenerPriorizacionesPendientes: obtenerPriorizacionesPendientes,
            obtenerPriorizionProyectoDetalleSGR: obtenerPriorizionProyectoDetalleSGR,
            GuardarPriorizionProyectoDetalleSGR: GuardarPriorizionProyectoDetalleSGR,
            obtenerErroresProyecto: obtenerErroresProyecto,
        };

        function consultarPriorizacionPorBPIN(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPriorizacionPorBPIN;            
            return $http.post(url, { "BPINs": [bpin], });
        }

        function SGR_Proyectos_MostrarEstadosPriorizacion(proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_MostrarEstadosPriorizacion + "?proyectoId=" + proyectoId;
            return $http.get(url);
        }

        function obtenerErroresPriorizacionSgr(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function obtenerPriorizacionesPendientes(idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPriorizacionProyecto;
            url += "?instanciaId=" + idInstancia;
            return $http.get(url);
        }
        function obtenerPriorizionProyectoDetalleSGR(instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_ObtenerPriorizionProyectoDetalleSGR + "?instanciaId=" + instanciaId;
            return $http.get(url);
        }
        function GuardarPriorizionProyectoDetalleSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_GuardarPriorizionProyectoDetalleSGR, model);
        }

        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }
    }
})();

