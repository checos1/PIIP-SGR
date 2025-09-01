(function () {
    'use strict';

    angular.module('backbone').factory('servicioAdherencia', servicioAdherencia);
    servicioAdherencia.$inject = ['$http', 'constantesBackbone'];

    function servicioAdherencia($http, constantesBackbone) {

        return {
            guardarAdherencia: guardarAdherencia,
            eliminarAdherencia: eliminarAdherencia,
            obtenerAdherenciasPorEntidadId: obtenerAdherenciasPorEntidadId,
            obtenerEntidades: obtenerEntidades,
        }

        function guardarAdherencia(adherencia) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAdherenciaGuardar;
            return $http.post(url, adherencia);
        }

        function eliminarAdherencia(adherencia) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAdherenciaEliminar + adherencia.AdherenciaId;
            return $http.post(url);
        }

        function obtenerAdherenciasPorEntidadId(IdEntidad) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAdherenciaObtenerPorEntidadId + IdEntidad;
            return $http.get(url);
        }

        function obtenerEntidades(peticion, tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadPorTipoEntidad + tipoEntidad;
            return $http.get(url);
        }
    }

})();