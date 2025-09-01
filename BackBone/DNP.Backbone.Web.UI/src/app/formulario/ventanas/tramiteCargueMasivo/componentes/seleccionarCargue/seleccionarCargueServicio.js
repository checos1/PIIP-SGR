(function () {
    'use strict';
    angular.module('backbone').factory('seleccionarCargueServicio', seleccionarCargueServicio);

    seleccionarCargueServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function seleccionarCargueServicio($q, $http, $location, constantesBackbone) {
        return {
        };
    }
})();