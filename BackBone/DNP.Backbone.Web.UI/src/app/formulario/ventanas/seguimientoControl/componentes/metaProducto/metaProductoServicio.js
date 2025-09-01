(function () {
    'use strict';
    angular.module('backbone').factory('metaProductoServicio', metaProductoServicio);

    metaProductoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function metaProductoServicio($q, $http, $location, constantesBackbone) {
        return {
        };
    }
})();