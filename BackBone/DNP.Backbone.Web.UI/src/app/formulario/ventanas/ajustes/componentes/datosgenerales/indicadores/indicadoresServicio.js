(function () {
    'use strict';
    angular.module('backbone').factory('indicadoresServicio', indicadoresServicio);

    indicadoresServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function indicadoresServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerIndicadoresProducto: ObtenerIndicadoresProducto,
            actualizarindicadores: actualizarindicadores,
            Obtenerindicadores: Obtenerindicadores,
            agregarIndicadorSecundario: agregarIndicadorSecundario,
            EliminarIndicadorProducto: EliminarIndicadorProducto,
            ActualizarMetaAjusteIndicador: ActualizarMetaAjusteIndicador
        };

        function ObtenerIndicadoresProducto(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerIndicadorProducto + "?bpin=" + bpin;
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
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarIndicadorSecundario;
            return $http.post(url, parametros);
        }

        function EliminarIndicadorProducto(IndicadorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarIndicadorProducto + "?indicadorId=" + IndicadorId;
            return $http.get(url);

        }

        function ActualizarMetaAjusteIndicador(Indicador) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarMetaAjusteIndicador;
            return $http.post(url, Indicador);

        }
    }
})();