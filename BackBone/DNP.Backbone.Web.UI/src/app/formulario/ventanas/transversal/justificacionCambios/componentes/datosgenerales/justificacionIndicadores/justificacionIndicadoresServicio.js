(function () {
    'use strict';
    angular.module('backbone').factory('justificacionIndicadoresServicio', justificacionIndicadoresServicio);

    justificacionIndicadoresServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function justificacionIndicadoresServicio($q, $http, $location, constantesBackbone) {
        return {
            IndicadoresValidarCapituloModificado: IndicadoresValidarCapituloModificado
        };

        function IndicadoresValidarCapituloModificado(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneIndicadoresValidarCapituloModificado + "?bpin=" + bpin;
            return $http.get(url);
        }
    }
})();