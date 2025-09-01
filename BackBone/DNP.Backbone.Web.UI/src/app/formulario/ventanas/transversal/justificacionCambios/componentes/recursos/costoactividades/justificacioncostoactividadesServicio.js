(function () {
    'use strict';
    angular.module('backbone').factory('justificacioncostoactividadesServicio', justificacioncostoactividadesServicio);

    justificacioncostoactividadesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function justificacioncostoactividadesServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerResumenObjetivosProductosActividadesJustificacion: ObtenerResumenObjetivosProductosActividadesJustificacion
        };
        function ObtenerResumenObjetivosProductosActividadesJustificacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenObjetivosProductosActividadesJustificacion + "?bpin=" + parametros;
            return $http.get(url);
        }
    }
})();