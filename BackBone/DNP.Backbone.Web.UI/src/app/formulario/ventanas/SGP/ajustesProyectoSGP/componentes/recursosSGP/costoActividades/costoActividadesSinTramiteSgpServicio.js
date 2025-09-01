(function () {
    'use strict';
    angular.module('backbone').factory('costoActividadesSinTramiteSgpServicio', costoActividadesSinTramiteSgpServicio);

    costoActividadesSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function costoActividadesSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerResumenObjetivosProductosActividades: ObtenerResumenObjetivosProductosActividades,
            Guardar: Guardar,
            AgregarEntregable: AgregarEntregable,
            EliminarEntregable: EliminarEntregable,
            ObtenerResumenObjetivosProductosActividadesSgp: ObtenerResumenObjetivosProductosActividadesSgp,
            GuardarSgp: GuardarSgp,
            AgregarEntregableSgp: AgregarEntregableSgp,
            EliminarEntregableSgp: EliminarEntregableSgp,
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
        //Sesion migracion de servicios Costos
        function ObtenerResumenObjetivosProductosActividadesSgp(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenObjetivosProductosActividadesSgp + "?bpin=" + parametros;
            return $http.get(url);
        }
        function GuardarSgp(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCostoActividadesSgp + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function AgregarEntregableSgp(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAgregarEntregableSgp + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function EliminarEntregableSgp(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarEntregableSgp + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }	
    }
})();