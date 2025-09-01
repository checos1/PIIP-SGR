(function () {
    'use strict';
    angular.module('backbone').factory('indicadoresSinTramiteSgpServicio', indicadoresSinTramiteSgpServicio);

    indicadoresSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function indicadoresSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerIndicadoresProducto: ObtenerIndicadoresProducto,
            actualizarindicadores: actualizarindicadores,
            Obtenerindicadores: Obtenerindicadores,
            agregarIndicadorSecundario: agregarIndicadorSecundario,
            EliminarIndicadorProducto: EliminarIndicadorProducto,
            ActualizarMetaAjusteIndicador: ActualizarMetaAjusteIndicador
        };

        function ObtenerIndicadoresProducto(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerIndicadorProductoSGP + "?bpin=" + bpin;
            return $http.get(url);

        }

        function Obtenerindicadores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEncabezadoGeneral;
            return $http.post(url, parametros);

        }

        function actualizarindicadores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarindicadores;
            return $http.post(url, parametros);
        }

        function agregarIndicadorSecundario(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarIndicadorSecundarioSGP;
            return $http.post(url, parametros);
        }

        function EliminarIndicadorProducto(IndicadorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarIndicadorProductoSGP + "?indicadorId=" + IndicadorId;
            return $http.get(url);

        }

        function ActualizarMetaAjusteIndicador(Indicador) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarMetaAjusteIndicadorSGP;
            return $http.post(url, Indicador);

        }
    }
})();