(function () {
    'use strict';
    angular.module('backbone').factory('reporteFinancieroServicio', reporteFinancieroServicio);

    reporteFinancieroServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function reporteFinancieroServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerPreguntasAvanceFinanciero: ObtenerPreguntasAvanceFinanciero,
            GuardarPreguntasAvanceFinanciero: GuardarPreguntasAvanceFinanciero,
            ObtenerAvanceFinanciero: ObtenerAvanceFinanciero,
            GuardarAvanceFinanciero: GuardarAvanceFinanciero,
        };

        function ObtenerPreguntasAvanceFinanciero(instancia, proyectoid, bpin, nivelid, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasAvanceFinanciero + "?instancia=" + instancia + "&proyectoid=" + proyectoid + "&bpin=" + bpin + "&nivelid=" + nivelid + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
		}

        function GuardarPreguntasAvanceFinanciero(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasAvanceFinanciero;
            return $http.post(url, parametros);
        }

        function ObtenerAvanceFinanciero(instancia, proyectoid, bpin, vigenciaId, periodoPeriodicidadId, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAvanceFinanciero + "?instancia=" + instancia + "&proyectoid=" + proyectoid + "&bpin=" + bpin + "&vigenciaId=" + vigenciaId + "&periodoPeriodicidadId=" + periodoPeriodicidadId + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function GuardarAvanceFinanciero(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarAvanceFinanciero;
            return $http.post(url, parametros);
        }

    }
})();