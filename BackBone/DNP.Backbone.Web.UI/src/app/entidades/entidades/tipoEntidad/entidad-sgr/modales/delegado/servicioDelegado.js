(function () {
    'use strict';

    angular.module('backbone').factory('servicioDelegado', servicioDelegado);
    servicioDelegado.$inject = ['$http', 'constantesBackbone'];

    function servicioDelegado($http, constantesBackbone) {

        return {
            guardarDelegado: guardarDelegado,
            eliminarDelegado: eliminarDelegado,
            obtenerDelegadosPorEntidadId: obtenerDelegadosPorEntidadId,
            obtenerUsuarios: obtenerUsuarios,
            obtenerUsuariosPorEntidad: obtenerUsuariosPorEntidad,
            obtenerEntidades: obtenerEntidades,
        }

        function guardarDelegado(delegado) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDelegadoGuardar;
            return $http.post(url, delegado);
        }

        function eliminarDelegado(delegado) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDelegadoEliminar + delegado.DelegadoId;
            return $http.post(url);
        }

        function obtenerDelegadosPorEntidadId(IdEntidad) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDelegadoObtenerPorEntidadId + IdEntidad;
            return $http.get(url);
        }

        function obtenerUsuarios() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuarios;
            return $http.get(url);
        }

        function obtenerEntidades() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesNegocio;
            return $http.get(url);
        }

        function obtenerUsuariosPorEntidad(tipoEntidad, filtro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosPorEntidad + tipoEntidad + '&filtro=' + filtro;
            return $http.get(url);
        }
    }
})();