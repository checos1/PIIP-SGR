(function () {
    'use strict';
    angular.module('backbone').factory('politicasTransversalesServicio', politicasTransversalesServicio);

    politicasTransversalesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function politicasTransversalesServicio($q, $http, $location, constantesBackbone) {
        return null;
        //{
        //    obtenerCostosPIIPvsFuentesPIIP: obtenerCostosPIIPvsFuentesPIIP
        //};


        //function obtenerCostosPIIPvsFuentesPIIP(Bpin, usuarioDNP, idFormulario) {
        //    var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCostosPIIPvsFuentesPIIP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
        //    return $http.get(url);
        //}
    }
})();