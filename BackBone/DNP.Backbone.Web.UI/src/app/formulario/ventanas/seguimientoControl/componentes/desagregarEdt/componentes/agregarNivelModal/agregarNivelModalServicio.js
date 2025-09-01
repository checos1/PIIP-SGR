(function () {
    'use strict';
    angular.module('backbone').factory('agregarNivelModalServicio', agregarNivelModalServicio);

    agregarNivelModalServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function agregarNivelModalServicio($q, $http, $location, constantesBackbone) {
        return {
            registrarNiveles: registrarNiveles
        };

        function registrarNiveles(model) {
            var data = {
                Tipo: "Nivel",
                NivelesNuevos: model
            }
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriRegistrarNivelesNuevos, data);
        }
    }
})();