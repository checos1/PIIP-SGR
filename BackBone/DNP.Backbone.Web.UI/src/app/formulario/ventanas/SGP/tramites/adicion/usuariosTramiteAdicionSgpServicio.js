(function () {
    'use strict';

    angular.module('backbone').factory('usuariosTramiteAdicionSgpServicio', usuariosTramiteAdicionSgpServicio);

    usuariosTramiteAdicionSgpServicio.$inject = ['$http', 'constantesBackbone'];

    function usuariosTramiteAdicionSgpServicio($http, constantesBackbone) {

        return {
            obtenerPreguntasPersonalizadas: obtenerPreguntasPersonalizadas,
            obtenerPreguntasPersonalizadasComponente: obtenerPreguntasPersonalizadasComponente,
            obtenerPreguntasPersonalizadasComponenteSGR: obtenerPreguntasPersonalizadasComponenteSGR,
            guardarRespuestas: guardarRespuestas,
            guardarRespuestasCustomSGR: guardarRespuestasCustomSGR,
            obtener: obtener,
            ConsultarAccionPorInstancia: ConsultarAccionPorInstancia,
            DevolverCuestionarioProyecto: DevolverCuestionarioProyecto,
            obtenerErroresviabilidadSgr: obtenerErroresviabilidadSgr,
            consultarFuentesSGR: consultarFuentesSGR,
            registrarFuentesSGR: registrarFuentesSGR,
            consultarFuentesNoSGR: consultarFuentesNoSGR,
            registrarFuentesNoSGR: registrarFuentesNoSGR,
            consultarResumenFuentesCostos: consultarResumenFuentesCostos,
            ObtenerTiposCofinanciaciones: ObtenerTiposCofinanciaciones,
            registrarDatosAdicionalesCofinanciadorFuentesNoSGR: registrarDatosAdicionalesCofinanciadorFuentesNoSGR,
            consultarDatosAdicionalesCofinanciadorNoSGR: consultarDatosAdicionalesCofinanciadorNoSGR,
            SGR_Acuerdo_LeerProyecto: SGR_Acuerdo_LeerProyecto,
            SGR_Acuerdo_GuardarProyecto: SGR_Acuerdo_GuardarProyecto,
            SGR_Proyectos_LeerListas: SGR_Proyectos_LeerListas,
            devolverProyectoSGP: devolverProyectoSGP,
            SGR_Viabilidad_LeerInformacionGeneral: SGR_Viabilidad_LeerInformacionGeneral,
            SGR_Viabilidad_LeerParametricas: SGR_Viabilidad_LeerParametricas,
            SGR_Viabilidad_GuardarInformacionBasica: SGR_Viabilidad_GuardarInformacionBasica,
            SGR_Viabilidad_ValidarCargueDocumentoObligatorio: SGR_Viabilidad_ValidarCargueDocumentoObligatorio,
            SGR_Viabilidad_obtenerConceptosPreviosEmitidos: SGR_Viabilidad_obtenerConceptosPreviosEmitidos
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

        function guardarRespuestas(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadas, parametros);
        }

        function guardarRespuestasCustomSGR(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasPersonalizadasCustomSGR, parametros);
        }

        function obtener(instanciaId, nivelId) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerFlujosLogInstancia}?instanciaId=${instanciaId}&nivelId=${nivelId}`;
            return $http.get(url);
        }

        function ConsultarAccionPorInstancia(instanciaId, idAccion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneConsultarAccionPorInstancia}?instanciaId=${instanciaId}&idAccion=${idAccion}`;
            return $http.get(url);
        }

        function DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneDevolverCuestionarioProyecto}?nivelId=${nivelId}&instanciaId=${instanciaId}&estadoAccionesPorInstancia=${estadoAccionesPorInstancia}`;
            return $http.post(url);
        }

        function consultarFuentesSGR(Bpin, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarFuentesSGR + "?bpin=" + Bpin + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function registrarFuentesSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneRegistrarFuentesSGR, model);
        }

        function consultarFuentesNoSGR(Bpin, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarFuentesNoSGR + "?bpin=" + Bpin + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function registrarFuentesNoSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneRegistrarFuentesNoSGR, model);
        }

        function consultarResumenFuentesCostos(Bpin, instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarResumenFuentesCostos + "?bpin=" + Bpin + "&instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function ObtenerTiposCofinanciaciones() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarTiposCofinanciaciones;
            return $http.get(url);
        }

        function registrarDatosAdicionalesCofinanciadorFuentesNoSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneRegistrarDatosAdicionalesCofinanciadorFuentesNoSGR, model);
        }

        function consultarDatosAdicionalesCofinanciadorNoSGR(Bpin, vigencia, vigenciaFuente) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarDatosAdicionalesCofinanciadorFuentesNoSGR + "?bpin=" + Bpin + "&vigencia=" + vigencia + "&vigenciaFuente=" + vigenciaFuente;
            return $http.get(url);
        }

        function SGR_Acuerdo_LeerProyecto(proyectoId, nivelId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Acuerdo_LeerProyecto + "?proyectoId=" + proyectoId + "&nivelId=" + nivelId;
            return $http.get(url);
        }

        function SGR_Acuerdo_GuardarProyecto(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Acuerdo_GuardarProyecto, data);
        }

        function devolverProyectoSGP(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDevolverProyectoSGP, parametros);
        }

        function SGR_Proyectos_LeerListas(nivelId, proyectoId, nombreLista) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerListas + "?nivelId=" + nivelId + "&proyectoId=" + proyectoId + "&nombreLista=" + nombreLista;
            return $http.get(url);
        }

        function SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Viabilidad_LeerInformacionGeneral + "?proyectoId=" + proyectoId + "&instanciaId=" + instanciaId + "&tipoConceptoViabilidadCode=" + tipoConceptoViabilidadCode;
            return $http.get(url);
        }

        function SGR_Viabilidad_LeerParametricas(proyectoId, nivelId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Viabilidad_LeerParametricas + "?proyectoId=" + proyectoId + "&nivelId=" + nivelId;
            return $http.get(url);
        }

        function SGR_Viabilidad_GuardarInformacionBasica(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Viabilidad_GuardarInformacionBasica, data);
        }

        function SGR_Viabilidad_ValidarCargueDocumentoObligatorio(data, coleccion) {
            return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarArchivosSgr}${coleccion}`, data);
        }

        function SGR_Viabilidad_obtenerConceptosPreviosEmitidos(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptosPreviosEmitidos + "?bpin=" + Bpin;
            return $http.get(url);
        }
    }
})();