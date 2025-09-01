(function () {
    'use strict';

    angular.module('backbone').factory('modalPNDServicio', modalPNDServicio);

    modalPNDServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function modalPNDServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPlanNacionalDesarrollo: obtenerPlanNacionalDesarrollo,
        };

        function obtenerPlanNacionalDesarrollo(idProyecto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPND;
            url += "?idProyecto=" + idProyecto;
            return $http.get(url);
        }
    }
})();