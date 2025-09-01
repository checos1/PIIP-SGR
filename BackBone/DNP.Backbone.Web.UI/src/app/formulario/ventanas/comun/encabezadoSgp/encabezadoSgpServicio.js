(function () {
    'use strict';

    angular.module('backbone').factory('encabezadoSgpServicio', encabezadoSgpServicio);

    encabezadoSgpServicio.$inject = ['$q', '$http', 'constantesBackbone'];


    function encabezadoSgpServicio($q, $http, constantesBackbone) {
        return {
            ObtenerEncabezadoSGP: ObtenerEncabezadoSGP,
        };

        function ObtenerEncabezadoSGP(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEncabezadoSGP;
            return $http.post(url, parametros);
        }
    }
})();