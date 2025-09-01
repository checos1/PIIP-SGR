(function () {
    'use strict';
    angular.module('backbone').factory('vigfuturaproductosServicio', vigfuturaproductosServicio);

    vigfuturaproductosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function vigfuturaproductosServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerProductosVigenciaFuturaConstante: ObtenerProductosVigenciaFuturaConstante,
            ObtenerProductosVigenciaFuturaCorriente: ObtenerProductosVigenciaFuturaCorriente,
            actualizarVigenciaFuturaProducto: actualizarVigenciaFuturaProducto
        };

        function ObtenerProductosVigenciaFuturaConstante(Bpin, TramiteId, AnioBase) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerProductosVigenciaFuturaConstante + "?Bpin=" + Bpin + "&TramiteId=" + TramiteId + "&AnioBase=" + AnioBase);
        }

        function ObtenerProductosVigenciaFuturaCorriente(Bpin, TramiteId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerProductosVigenciaFuturaCorriente + "?Bpin=" + Bpin + "&TramiteId=" + TramiteId);
        }

        function actualizarVigenciaFuturaProducto(fuente) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarVigenciaFuturaProducto;
            return $http.post(url, fuente);
        }
    }
})();