(function () {
    'use strict';
    angular.module('backbone').factory('costosFuentesAjustesServicio', costosFuentesAjustesServicio);

    costosFuentesAjustesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function costosFuentesAjustesServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerCostosPIIPvsFuentesPIIP: obtenerCostosPIIPvsFuentesPIIP
        };


        function obtenerCostosPIIPvsFuentesPIIP(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCostosPIIPvsFuentesPIIP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
    }
})();