(function () {
    'use strict';
    angular.module('backbone').factory('programarProdCapServicio', programarProdCapServicio);

    programarProdCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function programarProdCapServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListadoObjProdNiveles: obtenerListadoObjProdNiveles,
            GuardarProgramarProducto: GuardarProgramarProducto,

        };

        function obtenerListadoObjProdNiveles(Bpin, usuarioDNP) {            
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriObtenerListadoObjProdNivelesProgramarProducto + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function GuardarProgramarProducto(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarProgramarProducto;
            return $http.post(url, parametros);
        }

    }
})();