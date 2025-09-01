(function () {
    'use strict';
    angular.module('backbone').factory('aprobacionincorporacionServicio', aprobacionincorporacionServicio);

    aprobacionincorporacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function aprobacionincorporacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerPresupuestalProyectosAsociados: ObtenerPresupuestalProyectosAsociados
        };

        function ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPresupuestalProyectosAsociados + "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId;
            return $http.get(url);
        }
    }
})();