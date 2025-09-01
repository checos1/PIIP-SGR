(function () {
    'use strict';
    angular.module('backbone').factory('solicitudincorporacionServicio', solicitudincorporacionServicio);

    solicitudincorporacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function solicitudincorporacionServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerPresupuestalProyectosAsociados: ObtenerPresupuestalProyectosAsociados
        };

        function ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPresupuestalProyectosAsociados + "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId;
            return $http.get(url);
        }
    }
})();