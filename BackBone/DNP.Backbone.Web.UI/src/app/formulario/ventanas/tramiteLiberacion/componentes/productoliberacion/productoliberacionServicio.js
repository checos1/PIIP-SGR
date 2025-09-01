(function () {
    'use strict';
    angular.module('backbone').factory('productoliberacionServicio', productoliberacionServicio);

    productoliberacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function productoliberacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerValUtilizadosLiberacionVigenciasFuturas: ObtenerValUtilizadosLiberacionVigenciasFuturas,
            InsertaValoresproductosLiberacionVFCorrientes: InsertaValoresproductosLiberacionVFCorrientes,
            InsertaValoresproductosLiberacionVFConstantes: InsertaValoresproductosLiberacionVFConstantes
        };

        function ObtenerValUtilizadosLiberacionVigenciasFuturas(proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerValUtilizadosLiberacionVigenciasFuturas + "?proyectoId=" + proyectoId + "&tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function InsertaValoresproductosLiberacionVFCorrientes(autorizacion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneInsertaValoresproductosLiberacionVFCorrientes;
            return $http.post(url, autorizacion);
        }

        function InsertaValoresproductosLiberacionVFConstantes(autorizacion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneInsertaValoresproductosLiberacionVFConstantes;
            return $http.post(url, autorizacion);
        }
    }
})();