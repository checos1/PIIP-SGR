(function () {
    'use strict';
    angular.module('backbone').factory('vigfuturavalconstantesServicio', vigfuturavalconstantesServicio);

    vigfuturavalconstantesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function vigfuturavalconstantesServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerFuentesFinanciacionVigenciaFuturaConstante: ObtenerFuentesFinanciacionVigenciaFuturaConstante//,
            //actualizarVigenciaFuturaFuente: actualizarVigenciaFuturaFuente
        };

        function ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, tramiteId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerFuentesFinanciacionVigenciaFuturaConstante + "?bpin=" + bpin + "&tramiteId=" + tramiteId);
        }

        /*function actualizarVigenciaFuturaFuente(fuente) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarVigenciaFuturaFuente;
            return $http.post(url, fuente);
        }*/
    }
})();