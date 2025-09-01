(function () {
    'use strict';
    angular.module('backbone').factory('conceptoTecnicoServicio', conceptoTecnicoServicio);

    conceptoTecnicoServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function conceptoTecnicoServicio($q, $http, $location, constantesBackbone) {
        return {
            //CargarConpes: CargarConpes,
        };
        
    }
})();