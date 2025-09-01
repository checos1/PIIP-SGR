(function () {
    'use strict';

    angular.module('backbone').factory('trazabilidadModalServicio', trazabilidadModalServicio);

    trazabilidadModalServicio.$inject = ['$http', 'constantesBackbone'];

    function trazabilidadModalServicio($http, constantesBackbone) {
        return {
            obtener
        }

        function obtener(instanciaId, nivelId) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerFlujosLogInstancia}?instanciaId=${instanciaId}&nivelId=${nivelId}`;
            return $http.get(url);
        }
    }
})();