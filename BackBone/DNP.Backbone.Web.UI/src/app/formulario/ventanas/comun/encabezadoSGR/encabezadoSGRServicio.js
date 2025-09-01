(function () {
    'use strict';

    angular.module('backbone').factory('encabezadoSGRServicio', encabezadoSGRServicio);

    encabezadoSGRServicio.$inject = ['$q', '$http', 'constantesBackbone'];


    function encabezadoSGRServicio($q, $http, constantesBackbone) {
        return {
            ObtenerEncabezado: ObtenerEncabezado,
        };

        function ObtenerEncabezado(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEncabezadoSGR;
            return $http.post(url, parametros);
        }
    }
})();

