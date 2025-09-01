(function () {
    'use strict';
    angular.module('backbone').factory('beneficiariosSinTramiteSgpServicio', beneficiariosSinTramiteSgpServicio);

    beneficiariosSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function beneficiariosSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {

            ObtenerbeneficiariosTotales: ObtenerbeneficiariosTotales,
            ObtenerbeneficiariosTotalesDetalle: ObtenerbeneficiariosTotalesDetalle,
            GuardarBeneficiarioTotales: GuardarBeneficiarioTotales,
            GuardarBeneficiarioProducto: GuardarBeneficiarioProducto,
            GuardarBeneficiarioProductoLocalizacion: GuardarBeneficiarioProductoLocalizacion,
            GuardarBeneficiarioProductoLocalizacionCaracterizacion: GuardarBeneficiarioProductoLocalizacionCaracterizacion,
        };

        function ObtenerbeneficiariosTotales(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBeneficiariosSGP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerbeneficiariosTotalesDetalle(json, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBeneficiariosDetalleSGP + "?json=" + json + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function GuardarBeneficiarioTotales(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioTotalesSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function GuardarBeneficiarioProducto(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioProductoSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function GuardarBeneficiarioProductoLocalizacion(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioProductoLocalizacionSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function GuardarBeneficiarioProductoLocalizacionCaracterizacion(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioProductoLocalizacionCaracterizacionSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
    }
})();
