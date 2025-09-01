(function () {
    'use strict';
    angular.module('backbone').factory('agregarActividadModalServicio', agregarActividadModalServicio);

    agregarActividadModalServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function agregarActividadModalServicio($q, $http, $location, constantesBackbone) {
        return {
            registrarActividades: registrarActividades
        };

        function registrarActividades(model) {
            var data = {
                Tipo: "Actividad",
                NivelesNuevos: model
            }
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriRegistrarNivelesNuevos, data);
        }
    }
})();