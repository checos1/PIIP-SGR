(function () {
    'use strict';
    angular.module('backbone').factory('avalusoViabilidadSgrServicio', avalusoViabilidadSgrServicio);
    avalusoViabilidadSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function avalusoViabilidadSgrServicio($http, constantesBackbone) {
        return {
            notificarGuardado: notificarGuardado,
            notificarCambio: notificarCambio,
            SGR_Proyectos_RegistrarAvalUso: SGR_Proyectos_RegistrarAvalUso,
            SGR_Proyectos_LeerAvalUsoSgr: SGR_Proyectos_LeerAvalUsoSgr,
            obtenerErroresviabilidadAvalUsoSgr: obtenerErroresviabilidadAvalUsoSgr,
            SGR_Viabilidad_ValidarCargueDocumentoObligatorio: SGR_Viabilidad_ValidarCargueDocumentoObligatorio
        };

        function SGR_Proyectos_RegistrarAvalUso(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_RegistrarAvalUso, parametros);
        }

        function SGR_Proyectos_LeerAvalUsoSgr(proyectoId, instanciaId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerAvalUso + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId);
        }

        function notificarGuardado() { }

        function notificarCambio() { }

        function obtenerErroresviabilidadAvalUsoSgr(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function SGR_Viabilidad_ValidarCargueDocumentoObligatorio(data, coleccion) {
            return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarArchivosSgr}${coleccion}`, data);
        }


    }
})();