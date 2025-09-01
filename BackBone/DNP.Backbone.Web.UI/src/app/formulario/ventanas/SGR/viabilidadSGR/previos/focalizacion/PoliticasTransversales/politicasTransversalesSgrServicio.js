(function () {
    'use strict';
    angular.module('backbone').factory('politicasTransversalesSgrServicio', politicasTransversalesSgrServicio);

    politicasTransversalesSgrServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function politicasTransversalesSgrServicio($q, $http, $location, constantesBackbone) {
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