(function () {
    'use strict';
    angular.module('backbone').factory('justificacioncostoactividadesSinTramiteSgpServicio', justificacioncostoactividadesSinTramiteSgpServicio);

    justificacioncostoactividadesSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function justificacioncostoactividadesSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerResumenObjetivosProductosActividadesJustificacion: ObtenerResumenObjetivosProductosActividadesJustificacion
        };
        function ObtenerResumenObjetivosProductosActividadesJustificacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenObjetivosProductosActividadesJustificacion + "?bpin=" + parametros;
            return $http.get(url);
        }
    }
})();