(function () {
    'use strict';
    angular.module('backbone').factory('crucePoliticasServicio', crucePoliticasServicio);

    crucePoliticasServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function crucePoliticasServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerCruceProgramacionSeguimiento: ObtenerCruceProgramacionSeguimiento,
            GuardarCrucePoliticasSeguimiento: GuardarCrucePoliticasSeguimiento
        };

        function ObtenerCruceProgramacionSeguimiento(instanciaid, proyectoid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriapiBackboneObtenerCruceProgramacionSeguimiento + "?instanciaid=" + instanciaid + "&proyectoid=" + proyectoid;
            return $http.get(url);
        }

        function GuardarCrucePoliticasSeguimiento(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.uriapiBackboneGuardarCrucePoliticasSeguimiento + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
    }
})();