(function () {
    'use strict';
    angular.module('backbone').factory('autorizacionliberacionServicio', autorizacionliberacionServicio);

    autorizacionliberacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function autorizacionliberacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerLiberacionVigenciaFutura: ObtenerLiberacionVigenciaFutura,
            InsertaAutorizacionVigenciasFuturas: InsertaAutorizacionVigenciasFuturas
        };

        function ObtenerLiberacionVigenciaFutura(proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerLiberacionVigenciaFutura + "?proyectoId=" + proyectoId + "&tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function InsertaAutorizacionVigenciasFuturas(autorizacion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneInsertaAutorizacionVigenciasFuturas;
            return $http.post(url, autorizacion);
        }
    }
})();