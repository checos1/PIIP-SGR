(function () {
    'use strict';
    angular.module('backbone').factory('listadoProgramarActCapServicio', listadoProgramarActCapServicio);

    listadoProgramarActCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function listadoProgramarActCapServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListadoObjProdNiveles: obtenerListadoObjProdNiveles,
            eliminarNivel: eliminarNivel,
            Guardar: Guardar,
            obtenerUnidadesMedida: obtenerUnidadesMedida
        };

        function obtenerListadoObjProdNiveles(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerListadoObjProdNiveles, model);
        }

        function obtenerUnidadesMedida() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.uriObtenerUnidadesMedida);
        }

        function eliminarNivel(dataDelete) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriEliminarNivel, dataDelete);
        }

        function Guardar(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriActividadProgramacionSeguimientoPeriodosValores, model);
        }
    }
})();