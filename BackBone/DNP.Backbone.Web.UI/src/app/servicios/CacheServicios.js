(function (apiCache, apiCacheCatalogos) {
    'use strict';

    angular.module('backbone.core').factory('CacheServicios', CacheServicios);

    CacheServicios.$inject = ['$http', 'utilidades'];

    function CacheServicios($http, utilidades) {

        return {
            obtenerCatalogo: obtenerCatalogo
        }

        function obtenerCatalogo(url) {

            return $http.get(apiCache + apiCacheCatalogos + url)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }
    }


})(apiCache, apiCacheCatalogos);
