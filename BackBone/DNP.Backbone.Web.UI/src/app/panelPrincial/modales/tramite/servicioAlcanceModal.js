(function () {
    'use strict';

    angular.module('backbone').factory('servicioAlcanceModal', servicioAlcanceModal);

    servicioAlcanceModal.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function servicioAlcanceModal($q, $http, $location, constantesBackbone) {

        return {
            obtenerTipoMotivo: obtenerTipoMotivo,
            crearAlcance: crearAlcance
        }

        function obtenerTipoMotivo() {
            //return $http.get(apiPiipCore + constantesBackbone.apiBackboneObtenerTiposMotivoAnulacion);
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTiposMotivoAnulacion);
        }

        function crearAlcance(data) {
            //var url = apiPiipCore + constantesBackbone.apiBackboneCrearAlcance;
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCrearAlcance;
            return $http.post(url, data);
        }
    }
})();