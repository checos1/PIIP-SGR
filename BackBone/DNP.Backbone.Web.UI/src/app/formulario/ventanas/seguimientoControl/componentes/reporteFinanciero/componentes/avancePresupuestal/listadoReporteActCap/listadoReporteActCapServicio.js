(function () {
    'use strict';
    angular.module('backbone').factory('listadoReporteActCapServicio', listadoReporteActCapServicio);

    listadoReporteActCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function listadoReporteActCapServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListadoObjProdNiveles: obtenerListadoObjProdNiveles,
            eliminarNivel: eliminarNivel,
            Guardar: Guardar,
            obtenerUnidadesMedida: obtenerUnidadesMedida
        };

        function obtenerListadoObjProdNiveles(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerListadoObjProdNivelesReporte, model);
        }

        function obtenerUnidadesMedida() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerUnidadesMedida);
        }

        function eliminarNivel(dataDelete) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriEliminarNivel, dataDelete);
        }

        function Guardar(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriActividadReporteSeguimientoPeriodosValores, model);
        }
    }
})();