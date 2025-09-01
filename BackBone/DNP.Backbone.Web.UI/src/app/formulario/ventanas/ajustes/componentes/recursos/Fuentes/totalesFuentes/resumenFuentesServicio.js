(function () {
    'use strict';
    angular.module('backbone').factory('resumenFuentesServicio', resumenFuentesServicio);

    resumenFuentesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function resumenFuentesServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerResumenFuentesFinanciacion: obtenerResumenFuentesFinanciacion
        };


        function obtenerResumenFuentesFinanciacion(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneResumenFuenteFinanciacion + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
    }
})();