(function () {
    'use strict';
    angular.module('backbone').factory('desagregarEdtServicio', desagregarEdtServicio);

    desagregarEdtServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function desagregarEdtServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListadoObjProdNiveles: obtenerListadoObjProdNiveles,
            eliminarNivel: eliminarNivel,
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
    }
})();