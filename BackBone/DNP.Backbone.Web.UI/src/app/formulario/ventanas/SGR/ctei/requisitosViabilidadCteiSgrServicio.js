(function () {
    'use strict';

    angular.module('backbone').factory('requisitosViabilidadCteiSgrServicio', requisitosViabilidadCteiSgrServicio);

    requisitosViabilidadCteiSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function requisitosViabilidadCteiSgrServicio($http, constantesBackbone) {

        return {
            obtenerPreguntasPersonalizadas: obtenerPreguntasPersonalizadas,
            obtenerPreguntasPersonalizadasComponente: obtenerPreguntasPersonalizadasComponente,
            obtenerPreguntasPersonalizadasComponenteSGR: obtenerPreguntasPersonalizadasComponenteSGR,
            guardarRespuestas: guardarRespuestas,
            guardarRespuestasCustomSGR: guardarRespuestasCustomSGR,
            ConsultarAccionPorInstancia: ConsultarAccionPorInstancia,
            DevolverCuestionarioProyecto: DevolverCuestionarioProyecto,
            obtenerErroresviabilidadSgr: obtenerErroresviabilidadSgr,
            SGR_Acuerdo_LeerProyecto: SGR_Acuerdo_LeerProyecto,
            SGR_Acuerdo_GuardarProyecto: SGR_Acuerdo_GuardarProyecto,
            SGR_Proyectos_LeerListas: SGR_Proyectos_LeerListas,
            devolverProyecto: devolverProyecto,
            SGR_Proyectos_validarTecnicoOcadpaz: SGR_Proyectos_validarTecnicoOcadpaz,
            Flujos_SubPasoEjecutar: Flujos_SubPasoEjecutar,
            SGR_Proyectos_LeerDatosAdicionalesCTEI: SGR_Proyectos_LeerDatosAdicionalesCTEI,
            SGR_Proyectos_GuardarDatosAdicionalesCTEI: SGR_Proyectos_GuardarDatosAdicionalesCTEI
        };

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadas;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

        function obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadasComponente;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'nombreComponente': nombreComponente,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

        function obtenerPreguntasPersonalizadasComponenteSGR(bPin, nivelId, instanciaId, nombreComponente, listaRoles) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasPersonalizadasComponenteSGR;
            var params = {
                'bPin': bPin,
                'nivelId': nivelId,
                'instanciaId': instanciaId,
                'nombreComponente': nombreComponente,
                'listaRoles': listaRoles,
            };
            return $http.get(url, { params });
        }

        function obtenerErroresviabilidadSgr(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function guardarRespuestasCustomSGR(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadasCustomSGR, parametros);
        }

        function guardarRespuestas(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadas, parametros);
        }

        function ConsultarAccionPorInstancia(instanciaId, idAccion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneConsultarAccionPorInstancia}?instanciaId=${instanciaId}&idAccion=${idAccion}`;
            return $http.get(url);
        }

        function DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneDevolverCuestionarioProyecto}?nivelId=${nivelId}&instanciaId=${instanciaId}&estadoAccionesPorInstancia=${estadoAccionesPorInstancia}`;
            return $http.post(url);
        }

        function SGR_Acuerdo_LeerProyecto(proyectoId, nivelId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Acuerdo_LeerProyecto + "?proyectoId=" + proyectoId + "&nivelId=" + nivelId;
            return $http.get(url);
        }

        function SGR_Acuerdo_GuardarProyecto(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Acuerdo_GuardarProyecto, data);
        }

        function devolverProyecto(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDevolverProyecto, parametros);
        }

        function SGR_Proyectos_LeerListas(nivelId, proyectoId, nombreLista) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerListas + "?nivelId=" + nivelId + "&proyectoId=" + proyectoId + "&nombreLista=" + nombreLista;
            return $http.get(url);
        }

        function SGR_Proyectos_validarTecnicoOcadpaz(instanciaId, idAccion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_ValidarTecnicoOcadPaz + "?instanciaId=" + instanciaId + "&accionId=" + idAccion;
            return $http.get(url);
        }

        function Flujos_SubPasoEjecutar(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosSubPasoEjecutar, data);
        }

        function SGR_Proyectos_LeerDatosAdicionalesCTEI(proyectoId, instanciaId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_CTEI_LeerDatosAdicionalesCTEI + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId);
        }

        function SGR_Proyectos_GuardarDatosAdicionalesCTEI(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_CTEI_GuardarDatosAdicionalesCTEI, data)
        }
    }
})();