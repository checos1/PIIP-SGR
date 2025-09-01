(function () {
    'use strict';

    angular.module('backbone').factory('ctusAsignacionSgrServicio', ctusAsignacionSgrServicio);

    ctusAsignacionSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function ctusAsignacionSgrServicio($http, constantesBackbone) {

        return {
            registrarObservador: registrarObservador,
            limpiarObservadores: limpiarObservadores,
            notificarCambio: notificarCambio,
            ConsultarAccionPorInstancia: ConsultarAccionPorInstancia,
            DevolverCuestionarioProyecto: DevolverCuestionarioProyecto,
            obtenerErroresviabilidadSgr: obtenerErroresviabilidadSgr,
            SGR_CTUS_LeerProyectoCtusConcepto: SGR_CTUS_LeerProyectoCtusConcepto,
            SGR_CTUS_GuardarAsignacionUsuarioEncargado: SGR_CTUS_GuardarAsignacionUsuarioEncargado,
            SGR_Viabilidad_ValidarCargueDocumentoObligatorio: SGR_Viabilidad_ValidarCargueDocumentoObligatorio,
            SGR_Viabilidad_obtenerConceptosPreviosEmitidos: SGR_Viabilidad_obtenerConceptosPreviosEmitidos,
            ObtenerUsuariosEncargadosCtus: ObtenerUsuariosEncargadosCtus,
            SGR_CTUS_LeerProyectoCtusUsuarioEncargado: SGR_CTUS_LeerProyectoCtusUsuarioEncargado,
            SGR_Proyectos_LeerEntidadesAdscritas: SGR_Proyectos_LeerEntidadesAdscritas,
            SGR_Proyectos_ActualizarEntidadAdscritaCTUS: SGR_Proyectos_ActualizarEntidadAdscritaCTUS,
            SGR_Proyectos_ValidarEntidadDelegada: SGR_Proyectos_ValidarEntidadDelegada
        };

        /*Inicio - Usado para comunicar los componentes hijos*/
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
        /*Fin - Usado para comunicar los componentes hijos*/

        function obtenerErroresviabilidadSgr(guiMacroproceso, idProyecto, idNivel, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
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

        function SGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_CTUS_LeerProyectoCtusConcepto}?proyectoCtusId=${proyectoCtusId}`;
            return $http.get(url);
        }

        function SGR_CTUS_GuardarAsignacionUsuarioEncargado(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_CTUS_GuardarAsignacionUsuarioEncargado, data);
        }

        function SGR_Viabilidad_ValidarCargueDocumentoObligatorio(data, coleccion) {
            return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneValidarArchivosSgr}${coleccion}`, data);
        }

        function SGR_Viabilidad_obtenerConceptosPreviosEmitidos(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptosPreviosEmitidos + "?bpin=" + Bpin;
            return $http.get(url);
        }

        function ObtenerUsuariosEncargadosCtus(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerUsuariosInvolucrados, parametros);
        }

        function SGR_CTUS_LeerProyectoCtusUsuarioEncargado(proyectoCtusId, instanciaId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_CTUS_LeerProyectoCtusUsuarioEncargado}?proyectoCtusId=${proyectoCtusId}&instanciaId=${instanciaId}`;
            return $http.get(url);
        }

        //Servicios de delegar viabilidad

        function SGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_ValidarEntidadDelegada + "?proyectoId=" + proyectoId + "&tipo=" + tipo;
            return $http.get(url);
        }

        function SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_LeerEntidadesAdscritas + "?proyectoId=" + proyectoId + "&tipoEntidad=" + tipoEntidad;
            return $http.get(url);
        }

        function SGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, tipo) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneSGR_CTUS_ActualizarEntidadAdscritaCTUS}?proyectoId=${proyectoId}&entityId=${entityId}&tipo=${tipo}`;
            return $http.post(url);
        }

    }
})();