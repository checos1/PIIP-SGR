(function () {
    'use strict';
    angular.module('backbone').factory('costoActividadesServicio', costoActividadesServicio);

    costoActividadesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function costoActividadesServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerResumenObjetivosProductosActividades: ObtenerResumenObjetivosProductosActividades,
            Guardar: Guardar,
            AgregarEntregable: AgregarEntregable,
            EliminarEntregable: EliminarEntregable,
        };
        function ObtenerResumenObjetivosProductosActividades(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenObjetivosProductosActividades + "?bpin=" + parametros;
            return $http.get(url);
        }
        function Guardar(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCostoActividades + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function AgregarEntregable(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAgregarEntregable + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function EliminarEntregable(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarEntregable + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
    }
})();