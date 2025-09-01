(function () {
    'use strict';
    angular.module('backbone').factory('resumenliberacionServicio', resumenliberacionServicio);

    resumenliberacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function resumenliberacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerResumenLiberacionVigenciaFutura: ObtenerResumenLiberacionVigenciaFutura
        };

        function ObtenerResumenLiberacionVigenciaFutura(proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenLiberacionVigenciaFutura + "?proyectoId=" + proyectoId + "&tramiteId=" + tramiteId;
            return $http.get(url);
        }
    }
})();