(function () {
    'use strict';
    angular.module('backbone').factory('regionalizacionServicio', regionalizacionServicio);

    regionalizacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function regionalizacionServicio($q, $http, $location, constantesBackbone) {
        return {
            GuardarAvanceRegionalizacion: GuardarAvanceRegionalizacion,
        };

        function GuardarAvanceRegionalizacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarAvanceRegionalizacion;
            return $http.post(url, parametros);
        }
    }
})();