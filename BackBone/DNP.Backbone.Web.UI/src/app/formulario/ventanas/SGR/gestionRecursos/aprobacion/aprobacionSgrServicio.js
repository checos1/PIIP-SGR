(function () {
    'use strict';

    angular.module('backbone').factory('aprobacionSgrServicio', aprobacionSgrServicio);

    aprobacionSgrServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function aprobacionSgrServicio($q, $http, $location, constantesBackbone) {

        return {
            obtenerErroresProyecto: obtenerErroresProyecto,
            ObtenerProyectoAprobacionInstanciasSGR: ObtenerProyectoAprobacionInstanciasSGR,
            GuardarProyectoAprobacionInstanciasSGR: GuardarProyectoAprobacionInstanciasSGR,
            ObtenerProyectoResumenAprobacionSGR: ObtenerProyectoResumenAprobacionSGR,
            ObtenerProyectoResumenAprobacionCreditoParcialSGR: ObtenerProyectoResumenAprobacionCreditoParcialSGR,
            obtenerAprobacionProyectoCredito: obtenerAprobacionProyectoCredito,
            guardarAprobacionProyectoCredito: guardarAprobacionProyectoCredito,
            ObtenerProyectoResumenEstadoAprobacionCreditoSGR: ObtenerProyectoResumenEstadoAprobacionCreditoSGR
        };

        function GuardarProyectoAprobacionInstanciasSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarProyectoAprobacionInstanciasSGR, model);
        }

        function ObtenerProyectoAprobacionInstanciasSGR(idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoAprobacionInstanciasSGR;
            url += "?instanciaId=" + idInstancia;
            return $http.get(url);
        }

        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function ObtenerProyectoResumenAprobacionSGR(idProyecto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoResumenAprobacionSGR;
            url += "?proyectoId=" + idProyecto;
            return $http.get(url);
        }

        function ObtenerProyectoResumenAprobacionCreditoParcialSGR(idProyecto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoResumenAprobacionCreditoParcialSGR;
            url += "?proyectoId=" + idProyecto;
            return $http.get(url);
        }

        function obtenerAprobacionProyectoCredito(idInstancia, idEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_ObtenerAprobacionProyectoCredito;
            url += "?instancia=" + idInstancia;
            url += "&entidad=" + idEntidad;
            return $http.get(url);
        }

        function guardarAprobacionProyectoCredito(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_GuardarAprobacionProyectoCredito, parametros);
        }

        function ObtenerProyectoResumenEstadoAprobacionCreditoSGR(idProyecto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoResumenEstadoAprobacionCreditoSGR;
            url += "?proyectoId=" + idProyecto;
            return $http.get(url);
        }

    }
})();

