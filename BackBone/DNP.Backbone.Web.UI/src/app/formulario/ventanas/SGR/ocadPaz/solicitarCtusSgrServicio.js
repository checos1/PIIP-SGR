(function () {
    'use strict';

    angular.module('backbone').factory('solicitarCtusSgrServicio', solicitarCtusSgrServicio);

    solicitarCtusSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function solicitarCtusSgrServicio($http, constantesBackbone) {

        return {
            obtenerErroresviabilidadSgr: obtenerErroresviabilidadSgr,
            SGR_Viabilidad_ValidarCargueDocumentoObligatorio: SGR_Viabilidad_ValidarCargueDocumentoObligatorio,
            SGR_Proyectos_LeerProyectoCtus: SGR_Proyectos_LeerProyectoCtus,
            SGR_Proyectos_LeerEntidadesSolicitarCtus: SGR_Proyectos_LeerEntidadesSolicitarCtus,
            SGR_Proyectos_GuardarProyectoSolicitarCtus: SGR_Proyectos_GuardarProyectoSolicitarCtus,
        };

        function obtenerErroresviabilidadSgr(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function SGR_Viabilidad_ValidarCargueDocumentoObligatorio(data, coleccion) {
            return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarArchivosSgr}${coleccion}`, data);
        }    

        function SGR_Proyectos_LeerProyectoCtus(proyectoId, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerProyectoCtus + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function SGR_Proyectos_LeerEntidadesSolicitarCtus(proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerEntidadesSolicitarCtus + "?proyectoId=" + proyectoId ;
            return $http.get(url);
        }
        function SGR_Proyectos_GuardarProyectoSolicitarCtus(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_GuardarProyectoSolicitarCtus, data);
        }

    }
})();