(function () {
    'use strict';

    angular.module('backbone').factory('servicioActualizarFechasModal', servicioActualizarFechasModal);
    servicioActualizarFechasModal.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function servicioActualizarFechasModal($q, $http, $location, constantesBackbone) {

        return {
            Guardar: Guardar
        }

        function Guardar(programacion) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionGuardar;
            return $http.post(url, programacion);
        }
    }
})();