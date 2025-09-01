(function () {
    'use strict';
    angular.module('backbone').factory('catalogoServicio', catalogoServicio);

    catalogoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'utilidades'];
    


    function catalogoServicio($q, $http, $location, constantesBackbone, utilidades) {
        return {
            catalogoTodosTiposEntidades: catalogoTodosTiposEntidades,
        };

        function catalogoTodosTiposEntidades() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCatalogoTodosTipoEntidades;
            return $http.get(url);
        }
    }
})();