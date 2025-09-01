(function () {
    'use strict';

    angular.module('backbone').factory('ejecutorProSgrServicio', ejecutorProSgrServicio);

    ejecutorProSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function ejecutorProSgrServicio($http, constantesBackbone) {
        return {
            catalogoTodosTiposEntidades: catalogoTodosTiposEntidades,
            ObtenerEjecutorByTipoEntidad: ObtenerEjecutorByTipoEntidad,
            ObtenerEjecutores: ObtenerEjecutores,
            ObtenerEjecutoresAsociados: ObtenerEjecutoresAsociados,
            CrearEjecutorAsociado: CrearEjecutorAsociado,
            EliminarEjecutorAsociado: EliminarEjecutorAsociado,
        };

        function catalogoTodosTiposEntidades() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCatalogoTodosTipoEntidades;
            return $http.get(url);
        }

        function ObtenerEjecutorByTipoEntidad(idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEjecutorByTipoEntidad + "?idTipoEntidad=" + idTipoEntidad;
            return $http.get(url);
        }

        function ObtenerEjecutores(nit, tipoEntidadId, entidadId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEjecutores + "?nit=" + nit + "&tipoEntidadId=" + tipoEntidadId + "&entidadId=" + entidadId;
            return $http.get(url);
        }

        function ObtenerEjecutoresAsociados(proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEjecutoresAsociados + "?proyectoId=" + proyectoId;
            return $http.get(url);
        }

        function CrearEjecutorAsociado(proyectoId, ejecutorId, tipoEjecutorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCrearEjecutorAsociado + "?proyectoId=" + proyectoId + "&ejecutorId=" + ejecutorId + "&tipoEjecutorId=" + tipoEjecutorId;
            return $http.post(url);
        }

        function EliminarEjecutorAsociado(EjecutorAsociadoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarEjecutorAsociado + "?EjecutorAsociadoId=" + EjecutorAsociadoId;
            return $http.post(url);
        }
    }

})();