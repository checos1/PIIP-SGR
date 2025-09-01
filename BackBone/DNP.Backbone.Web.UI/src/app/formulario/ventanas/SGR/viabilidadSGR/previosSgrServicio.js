(function () {
    'use strict';

    angular.module('backbone').factory('previosSgrServicio', previosSgrServicio);

    previosSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function previosSgrServicio($http, constantesBackbone) {

        return {
            obtenerPreguntasPersonalizadas: obtenerPreguntasPersonalizadas,
            obtenerPreguntasPersonalizadasComponente: obtenerPreguntasPersonalizadasComponente,
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
            devolverProyecto: devolverProyecto,
            obtenerDesagregarRegionalizacion: obtenerDesagregarRegionalizacion,
            guardarCambiosFirme: guardarCambiosFirme,     
            actualizarDesagregarRegionalizacion: actualizarDesagregarRegionalizacion,            
            obtenerFocalizacionPoliticasTransversalesFuentes: obtenerFocalizacionPoliticasTransversalesFuentes,
            guardarFocalizacionCategoriasAjustesSgr: guardarFocalizacionCategoriasAjustesSgr,
            obtenerPoliticasTransversalesCrucePoliticasSgr: obtenerPoliticasTransversalesCrucePoliticasSgr,
            obtenerDatosIndicadoresPoliticaSgr: obtenerDatosIndicadoresPoliticaSgr,
            obtenerErroresProyecto: obtenerErroresProyecto,
            obtenerProyectos: obtenerProyectos,
            validarInstanciaCTUSNoFinalizada: validarInstanciaCTUSNoFinalizada,
            registrarObservador: registrarObservador,
            notificarCambio: notificarCambio,
            limpiarObservadores: limpiarObservadores
        };

        var observadores = [];

        function registrarObservador(callback) {
            if (!observadores) {
                observadores = [];
            }
            observadores.push(callback);
        }

        function notificarCambio(datos) {
            observadores.forEach(function (observador) {
                observador(datos);
            });
        }

        function limpiarObservadores() {
            observadores = [];
        }

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

        function devolverProyecto(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDevolverProyecto, parametros);
        }
        //Regionalizacion
        function obtenerDesagregarRegionalizacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDesagregarRegionalizacionSgr + "?bpin=" + Bpin;
            return $http.get(url);
        }
        function guardarCambiosFirme(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCambiosFirme;
            return $http.post(url, data);
        }
        function actualizarDesagregarRegionalizacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDesagregarRegionalizacionActualizar;
            return $http.post(url, parametros);
        }
        //Focalizacion
        function obtenerFocalizacionPoliticasTransversalesFuentes(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerFocalizacionPoliticasTransversalesFuentesSgr + "?bpin=" + bpin;
            return $http.get(url);
        }
        function guardarFocalizacionCategoriasAjustesSgr(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarFocalizacionCategoriasAjustesSgr + "?bpin=" + bpin;
            return $http.get(url);
        }
        function obtenerPoliticasTransversalesCrucePoliticasSgr(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesCrucePoliticasSgr + "?bpin=" + bpin;
            return $http.get(url);
        }
        function obtenerDatosIndicadoresPoliticaSgr(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerDatosIndicadoresPoliticaSgr + "?bpin=" + bpin;
            return $http.get(url);
        }
        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function obtenerProyectos(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBpin + "?bpin=" + bpin;
            return $http.post(url);
        }

        function validarInstanciaCTUSNoFinalizada(idProyecto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneValidarInstanciaCTUSNoFinalizada + "?idProyecto=" + idProyecto;
            return $http.get(url);
        }
    }
})();