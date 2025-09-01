(function () {
    'use strict';
    angular.module('backbone').factory('beneficiariosServicio', beneficiariosServicio);

    beneficiariosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function beneficiariosServicio($q, $http, $location, constantesBackbone) {
        return {

            ObtenerbeneficiariosTotales: ObtenerbeneficiariosTotales,
            ObtenerbeneficiariosTotalesDetalle: ObtenerbeneficiariosTotalesDetalle,
            GuardarBeneficiarioTotales: GuardarBeneficiarioTotales,
            GuardarBeneficiarioProducto: GuardarBeneficiarioProducto,
            GuardarBeneficiarioProductoLocalizacion: GuardarBeneficiarioProductoLocalizacion,
            GuardarBeneficiarioProductoLocalizacionCaracterizacion: GuardarBeneficiarioProductoLocalizacionCaracterizacion,
        };

        function ObtenerbeneficiariosTotales(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBeneficiarios + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerbeneficiariosTotalesDetalle(json, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBeneficiariosDetalle + "?json=" + json + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function GuardarBeneficiarioTotales(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioTotales + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function GuardarBeneficiarioProducto(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioProducto + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function GuardarBeneficiarioProductoLocalizacion(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioProductoLocalizacion + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function GuardarBeneficiarioProductoLocalizacionCaracterizacion(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarBeneficiarioProductoLocalizacionCaracterizacion + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
    }
})();
