(function () {
    'use strict';
    angular.module('backbone').factory('recursosregionalizacionSinTramiteSgpServicio', recursosregionalizacionSinTramiteSgpServicio);

    recursosregionalizacionSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function recursosregionalizacionSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerDetalleAjustesJustificaionRegionalizacion: obtenerDetalleAjustesJustificaionRegionalizacion,
        };

        function obtenerDetalleAjustesJustificaionRegionalizacion(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAjustesJustificaionRegionalizacion + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }


    }
})();