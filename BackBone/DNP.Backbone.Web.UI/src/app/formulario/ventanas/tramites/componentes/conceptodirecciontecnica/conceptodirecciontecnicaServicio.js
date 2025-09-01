(function () {
    'use strict';
    angular.module('backbone').factory('conceptodirecciontecnicaServicio', conceptodirecciontecnicaServicio);

    conceptodirecciontecnicaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function conceptodirecciontecnicaServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerConceptoDireccionTecnicaTramite: ObtenerConceptoDireccionTecnicaTramite,
            GuardarConceptoDireccionTecnicaTramite: GuardarConceptoDireccionTecnicaTramite,
        };

        function ObtenerConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptoDireccionTecnicaTramite, peticion);
        }
        function GuardarConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarConceptoDireccionTecnicaTramite, peticion);
        }
    }
})();


