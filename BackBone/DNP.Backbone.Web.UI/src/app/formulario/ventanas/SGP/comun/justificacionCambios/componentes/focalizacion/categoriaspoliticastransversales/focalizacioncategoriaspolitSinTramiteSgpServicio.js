(function () {
    'use strict';
    angular.module('backbone').factory('focalizacioncategoriaspolitSinTramiteSgpServicio', focalizacioncategoriaspolitSinTramiteSgpServicio);

    focalizacioncategoriaspolitSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function focalizacioncategoriaspolitSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerSeccionOtrasPoliticasFacalizacionPT: obtenerSeccionOtrasPoliticasFacalizacionPT,
            obtenerDetalleAjustesJustificaionFacalizacionPT: obtenerDetalleAjustesJustificaionFacalizacionPT,
            ObtenerSeccionPoliticaFocalizacionDT: ObtenerSeccionPoliticaFocalizacionDT,
            ObtenerCategoriasFocalizacionJustificacionSgp: ObtenerCategoriasFocalizacionJustificacionSgp,
            ObtenerDetalleCategoriasFocalizacionJustificacionSgp: ObtenerDetalleCategoriasFocalizacionJustificacionSgp,
        };

        function obtenerDetalleAjustesJustificaionFacalizacionPT(Bpin, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneobtenerDetalleAjustesJustificaionFacalizacionPT;
            url += "?Bpin=" + Bpin;
            url += "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function obtenerSeccionOtrasPoliticasFacalizacionPT(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneobtenerSeccionOtrasPoliticasFacalizacionPT + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerSeccionPoliticaFocalizacionDT(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSeccionPoliticasDT + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerCategoriasFocalizacionJustificacionSgp(Bpin, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerCategoriasFocalizacionJustificacionSgp;
            url += "?Bpin=" + Bpin;
            url += "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function ObtenerDetalleCategoriasFocalizacionJustificacionSgp(Bpin, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerDetalleCategoriasFocalizacionJustificacionSgp;
            url += "?Bpin=" + Bpin;
            url += "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

    }
})();