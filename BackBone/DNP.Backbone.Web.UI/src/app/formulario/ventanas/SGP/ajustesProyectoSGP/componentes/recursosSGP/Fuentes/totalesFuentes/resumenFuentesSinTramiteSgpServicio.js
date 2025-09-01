(function () {
    'use strict';
    angular.module('backbone').factory('resumenFuentesSinTramiteSgpServicio', resumenFuentesSinTramiteSgpServicio);

    resumenFuentesSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function resumenFuentesSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerResumenFuentesFinanciacion: obtenerResumenFuentesFinanciacion
        };


        function obtenerResumenFuentesFinanciacion(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneResumenFuenteFinanciacion + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
    }
})();