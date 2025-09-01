(function () {
    'use strict';
    angular.module('backbone').factory('seguimientoFocalizacionServicio', seguimientoFocalizacionServicio);

    seguimientoFocalizacionServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function seguimientoFocalizacionServicio($q, $http, $location, constantesBackbone) {
        return {
            GuardarAvanceRegionalizacion: GuardarAvanceRegionalizacion,
        };

        function GuardarAvanceRegionalizacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarAvanceRegionalizacion;
            return $http.post(url, parametros);
        }
    }
})();