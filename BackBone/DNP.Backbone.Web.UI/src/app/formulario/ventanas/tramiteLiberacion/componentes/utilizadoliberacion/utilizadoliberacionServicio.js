(function () {
    'use strict';
    angular.module('backbone').factory('utilizadoliberacionServicio', utilizadoliberacionServicio);

    utilizadoliberacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function utilizadoliberacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerLiberacionVigenciaFutura: ObtenerLiberacionVigenciaFutura,
            InsertaValoresUtilizadosLiberacionVF: InsertaValoresUtilizadosLiberacionVF
        };

        function ObtenerLiberacionVigenciaFutura(proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerLiberacionVigenciaFutura + "?proyectoId=" + proyectoId + "&tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function InsertaValoresUtilizadosLiberacionVF(autorizacion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneInsertaValoresUtilizadosLiberacionVF;
            return $http.post(url, autorizacion);
        }
    }
})();