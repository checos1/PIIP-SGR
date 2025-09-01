(function () {
    'use strict';

    angular.module('backbone').factory('operacionesCreditoSgrServicio', operacionesCreditoSgrServicio);

    operacionesCreditoSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function operacionesCreditoSgrServicio($http, constantesBackbone) {

        return {
            obtenerOperacionCreditoDatosGenerales: obtenerOperacionCreditoDatosGenerales,
            guardarOperacionCreditoDatosGenerales: guardarOperacionCreditoDatosGenerales,
            eliminarOperacionCredito: eliminarOperacionCredito,
            obtenerOperacionCreditoDetalles: obtenerOperacionCreditoDetalles,
            guardarOperacionCreditoDetalles: guardarOperacionCreditoDetalles,
            obtenerTiposRecursosEntidadPorGrupoRecursos: obtenerTiposRecursosEntidadPorGrupoRecursos
        };

        function obtenerOperacionCreditoDatosGenerales(Bpin, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerOperacionCreditoDatosGenerales + "?bpin=" + Bpin + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function guardarOperacionCreditoDatosGenerales(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarOperacionCreditoDatosGenerales, parametros);
        }

        function eliminarOperacionCredito(proyectoid) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarOperacionCredito + "?proyectoid=" + proyectoid);
        }

        function obtenerOperacionCreditoDetalles(Bpin, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerOperacionCreditoDetalles + "?bpin=" + Bpin + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function guardarOperacionCreditoDetalles(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarOperacionCreditoDetalles, parametros);
        }

        function obtenerTiposRecursosEntidadPorGrupoRecursos(entityTypeCatalogId, resourceGroupId, incluir) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTiposRecursosEntidadPorGrupoRecursos + "?entityTypeCatalogId=" + entityTypeCatalogId + "&resourceGroupId=" + resourceGroupId + "&incluir=" + incluir);
        }
    }
})();