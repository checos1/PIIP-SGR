(function () {
    'use strict';
    angular.module('backbone').factory('vigfuturavalcorrientesServicio', vigfuturavalcorrientesServicio);

    vigfuturavalcorrientesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function vigfuturavalcorrientesServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerFuentesFinanciacionVigenciaFuturaCorriente: ObtenerFuentesFinanciacionVigenciaFuturaCorriente,
            actualizarVigenciaFuturaFuente: actualizarVigenciaFuturaFuente,
        };

        function ObtenerFuentesFinanciacionVigenciaFuturaCorriente(bpin) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerFuentesFinanciacionVigenciaFuturaCorriente + "?bpin=" + bpin);
        }

        function actualizarVigenciaFuturaFuente(fuente) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarVigenciaFuturaFuente;
            return $http.post(url, fuente);
        }
    }
})();