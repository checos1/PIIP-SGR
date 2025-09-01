(function () {
    'use strict';
    angular.module('backbone').factory('justificacionIndicadoresSinTramiteSgpServicio', justificacionIndicadoresSinTramiteSgpServicio);

    justificacionIndicadoresSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function justificacionIndicadoresSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            IndicadoresValidarCapituloModificado: IndicadoresValidarCapituloModificado
        };

        function IndicadoresValidarCapituloModificado(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneIndicadoresValidarCapituloModificado + "?bpin=" + bpin;
            return $http.get(url);
        }
    }
})();