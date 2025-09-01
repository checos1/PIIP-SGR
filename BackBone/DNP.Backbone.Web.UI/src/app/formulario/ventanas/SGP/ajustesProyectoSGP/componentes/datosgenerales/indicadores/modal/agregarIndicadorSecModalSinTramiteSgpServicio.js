(function () {
    'use strict';

    angular.module('backbone').factory('agregarIndicadorSecModalSinTramiteSgpServicio', agregarIndicadorSecModalSinTramiteSgpServicio);
    agregarIndicadorSecModalSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function agregarIndicadorSecModalSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {

        return {
            Guardar: Guardar
        }

        function Guardar(programacion) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionGuardar;
            return $http.post(url, programacion);
        }
    }
})();