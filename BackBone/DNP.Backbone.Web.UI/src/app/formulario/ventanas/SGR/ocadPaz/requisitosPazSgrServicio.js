(function () {
    'use strict';

    angular.module('backbone').factory('requisitosPazSgrServicio', requisitosPazSgrServicio);

    requisitosPazSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function requisitosPazSgrServicio($http, constantesBackbone) {

        return {
            obtenerUsuariosVerificacionOcadPaz: obtenerUsuariosVerificacionOcadPaz,
            SGR_OCADPaz_GuardarAsignacionUsuarioEncargado: SGR_OCADPaz_GuardarAsignacionUsuarioEncargado,
            Flujos_SubPasoEjecutar: Flujos_SubPasoEjecutar,
            SGR_OCADPaz_GenerarFichaVerificacion: SGR_OCADPaz_GenerarFichaVerificacion
        };

        function obtenerUsuariosVerificacionOcadPaz(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerUsuariosVerificacionOcadPaz, parametros);
        }

        function SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_OCADPaz_GuardarAsignacionUsuarioEncargado, data);
        }

        function Flujos_SubPasoEjecutar(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosSubPasoEjecutar, data);
        }

        function SGR_OCADPaz_GenerarFichaVerificacion(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_OCADPaz_GenerarFichaVerificacion, data);
        }
    }
})();