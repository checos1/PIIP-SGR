(function () {
    'use strict';

    angular.module('backbone').factory('gestionRecursosSGPServicio', gestionRecursosSGPServicio);

    gestionRecursosSGPServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function gestionRecursosSGPServicio($q, $http, $location, constantesBackbone) {
        return {
            ObtenerLocalizacionProyecto: obtenerLocalizacionProyecto,
            obtenerFuentesFinanciacionVigenciaProyectoSGP: obtenerFuentesFinanciacionVigenciaProyectoSGP,
            eliminarFuentesFinanciacionProyectoSGP: eliminarFuentesFinanciacionProyectoSGP,
            obtenerFuentesProgramarSolicitadoSGP: obtenerFuentesProgramarSolicitadoSGP,
            agregarFuentesProgramarSolicitadoSGP: agregarFuentesProgramarSolicitadoSGP,
            guardarCambiosFirmeSGP: guardarCambiosFirmeSGP,
            eliminarCapitulosModificadosSGP: eliminarCapitulosModificadosSGP,
            obtenerErroresProyectoSGP: obtenerErroresProyectoSGP,            
            obtenerTipoCofinanciador: obtenerTipoCofinanciador,
            obtenerFondos: obtenerFondos,
            obtenerRubros: obtenerRubros,
            obtenerProyectos: obtenerProyectos,
            obtenerListaEntidades: obtenerListaEntidades,
            obtenerListaEtapas: obtenerListaEtapas,
            obtenerListaTipoEntidad: obtenerListaTipoEntidad,
            obtenerSectores: obtenerSectores,
            ObtenerListaTiposRecursosxEntidad: ObtenerListaTiposRecursosxEntidad,
            ObtenerListaTiposRecursosxEntidadSgp: ObtenerListaTiposRecursosxEntidadSgp,
            obtenerDatosAdicionalesSGP: obtenerDatosAdicionalesSGP,
            agregarDatosAdicionalesSGP: agregarDatosAdicionalesSGP,
            eliminarDatosAdicionalesSGP: eliminarDatosAdicionalesSGP,
            agregarFuentesFinanciacionProyectoSGP: agregarFuentesFinanciacionProyectoSGP,
            obtenerDesagregarRegionalizacionSGP: obtenerDesagregarRegionalizacionSGP,
            actualizarDesagregarRegionalizacionSGP: actualizarDesagregarRegionalizacionSGP,
            obtenerPoliticasTransversalesCrucePoliticasSGP: obtenerPoliticasTransversalesCrucePoliticasSGP,
            actualizarPoliticasTransversalesCrucePoliticasSGP: actualizarPoliticasTransversalesCrucePoliticasSGP,
            obtenerDatosCPP: obtenerDatosCPP,
            guardarDatosSolicitudRecursosSgp: guardarDatosSolicitudRecursosSgp,
            obtenerDatosIndicadoresPoliticaSgp: obtenerDatosIndicadoresPoliticaSgp,
            obtenerResumenCostosVsSolicitadoSgp: obtenerResumenCostosVsSolicitadoSgp
        };

        function obtenerLocalizacionProyecto(Bpin) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaLocalizacionesSGP + "?bpin=" + Bpin);
        }

        function obtenerFuentesFinanciacionVigenciaProyectoSGP(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionVigenciaConsultarSGP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function eliminarFuentesFinanciacionProyectoSGP(fuenteId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionEliminarSGP + "?fuenteId=" + fuenteId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function obtenerFuentesProgramarSolicitadoSGP(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuentesProgramarSolicitadoSGP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function agregarFuentesProgramarSolicitadoSGP(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarFuentesProgramarSolicitadoSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function guardarCambiosFirmeSGP(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCambiosFirmeRelacionPlanificacion;
            return $http.post(url, data);
        }

        function eliminarCapitulosModificadosSGP(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonEliminarCambiosFirme;
            return $http.post(url, data);
        }

        function obtenerErroresProyectoSGP(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }        

        function obtenerTipoCofinanciador(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTipoCofinanciador;
            return $http.post(url, peticion);
        }

        function obtenerFondos(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaFondos;
            return $http.post(url, peticion);
        }

        function obtenerRubros(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaRubros;
            return $http.post(url, peticion);
        }

        function obtenerProyectos(peticion, bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBpin + "?bpin=" + bpin;
            return $http.post(url, peticion);
        }

        function obtenerListaEntidades(peticionobtenerProyectos, idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidadesTotal;
            return $http.post(url, peticionobtenerProyectos);
        }

        function obtenerListaEtapas(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEtapas;
            return $http.post(url, parametros);
        }

        function obtenerListaTipoEntidad(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoTipoEntidad;
            return $http.post(url, parametros);
        }

        function obtenerSectores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaSectoresEntity;
            return $http.post(url, parametros);
        }

        function ObtenerListaTiposRecursosxEntidad(peticion, idEntitytype) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTiposRecursosxEntidad + "?entityTypeCatalogId=" + idEntitytype;
            return $http.post(url, peticion);
        }
        //Migracion de servicio de fuentes de financiacion
        function ObtenerListaTiposRecursosxEntidadSgp(peticion, entityTypeCatalogId, entityType) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTiposRecursosxEntidadSgp + "?entityTypeCatalogId=" + entityTypeCatalogId + "&entityType=" + entityType;
            return $http.post(url, peticion);
        }
        function obtenerDatosAdicionalesSGP(fuenteId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesConsultarSGP + "?fuenteId=" + fuenteId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function agregarDatosAdicionalesSGP(parametros, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesAgregarSGP + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function eliminarDatosAdicionalesSGP(cofinanciadorId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesEliminarSGP + "?cofinanciadorId=" + cofinanciadorId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function agregarFuentesFinanciacionProyectoSGP(parametros, usuarioDNP, idFormulario, idInstancia, idAccion, idAplicacion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionAgregarSGP + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function obtenerDesagregarRegionalizacionSGP(Bpin) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDesagregarRegionalizacionSGP + "?bpin=" + Bpin);
        }

        function actualizarDesagregarRegionalizacionSGP(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDesagregarRegionalizacionActualizar;
            return $http.post(url, parametros);
        }

        function obtenerPoliticasTransversalesCrucePoliticasSGP(bpin, idFuente) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesCrucePoliticas;
            url += "?bpin=" + bpin;
            url += "&idfuente=" + idFuente;
            return $http.get(url);
        }

        function actualizarPoliticasTransversalesCrucePoliticasSGP(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizarPoliticasTransversalesCrucePoliticas + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function obtenerDatosCPP(bpin, idFuente, politicaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProductosPoliticasSgp;
            url += "?bpin=" + bpin;
            url += "&fuenteId=" + idFuente;
            url += "&politicaId=" + politicaId;
            return $http.get(url);
        }

        function guardarDatosSolicitudRecursosSgp(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarDatosSolicitudRecursosSgp;
            return $http.post(url, parametros);
        }

        function obtenerDatosIndicadoresPoliticaSgp(id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosIndicadoresPoliticasSgp;
            url += "?bpin=" + id;
            return $http.get(url);
        }

        function obtenerResumenCostosVsSolicitadoSgp(Bpin, usuarioDNP, idFormulario) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneResumenCostosVsSolicitado + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
    }

})();