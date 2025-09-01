(function () {
    'use strict';

    angular.module('backbone').factory('procesosSgrServicio', procesosSgrServicio);

    procesosSgrServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function procesosSgrServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPriorizacionesPendientes: obtenerPriorizacionesPendientes,
            obtenerAprobacionesPendientes: obtenerAprobacionesPendientes,
        };

        function obtenerPriorizacionesPendientes(idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPriorizacionProyecto;
            url += "?instanciaId=" + idInstancia;
            return $http.get(url);
        }

        function obtenerAprobacionesPendientes(idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAprobacionProyecto;
            url += "?instanciaId=" + idInstancia;
            return $http.get(url);
        }
    }
})();
