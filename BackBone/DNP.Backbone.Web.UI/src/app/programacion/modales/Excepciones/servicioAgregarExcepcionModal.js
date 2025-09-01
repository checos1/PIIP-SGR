(function () {
    'use strict';

    angular.module('backbone').factory('servicioAgregarExcepcionModal', servicioAgregarExcepcionModal);
    servicioAgregarExcepcionModal.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function servicioAgregarExcepcionModal($q, $http, $location, constantesBackbone) {

        return {
            obtenerExcepciones: obtenerExcepciones,
            guardar: guardar,
            modificar: modificar,
            eliminar: eliminar,
            obtenerEntidades: obtenerEntidades
        }

        function obtenerExcepciones(idProgramacion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneProgramacionExcepcionObtener}?idProgramacion=${idProgramacion}`;
            return $http.get(url);
        }

        function guardar(programacionExcepcion) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionExcepcionGuardar;
            return $http.post(url, programacionExcepcion);
        }

        function modificar(programacionExcepcion) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionExcepcionEditar;
            return $http.post(url, programacionExcepcion);
        }

        function eliminar(programacionExcepcion) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionExcepcionEliminar;
            return $http.post(url, programacionExcepcion);
        }

        function obtenerEntidades(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadPorTipoEntidad + tipoEntidad;
            return $http.get(url);
        }
    }
})();