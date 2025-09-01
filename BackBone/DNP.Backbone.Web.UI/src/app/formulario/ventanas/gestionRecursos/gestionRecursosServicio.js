(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('gestionRecursosServicio', gestionRecursosServicio);

    gestionRecursosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function gestionRecursosServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerDatosGeneralesProyecto: obtenerDatosGeneralesProyecto,
            ObtenerTokenMGA: obtenerTokenMGA,
            ObtenerLocalizacionProyecto: obtenerLocalizacionProyecto,
            obtenerListaEntidades: obtenerListaEntidades,
            obtenerListaEtapas: obtenerListaEtapas,
            obtenerListaTiposEntidades: obtenerListaTiposEntidades,
            obtenerListaTipoEntidad: obtenerListaTipoEntidad,
            obtenerSectores: obtenerSectores,
            obtenerListaRecursos: obtenerListaRecursos,
            obtenerFuentesFinanciacionProyecto: obtenerFuentesFinanciacionProyecto,
            obtenerFuentesFinanciacionVigenciaProyecto: obtenerFuentesFinanciacionVigenciaProyecto,
            obtenerTipoCofinanciador: obtenerTipoCofinanciador,
            obtenerFondos: obtenerFondos,
            obtenerRubros: obtenerRubros,
            obtenerProyectos: obtenerProyectos,
            agregarFuentesFinanciacionProyecto: agregarFuentesFinanciacionProyecto,
            eliminarFuentesFinanciacionProyecto: eliminarFuentesFinanciacionProyecto,
            obtenerDesagregarRegionalizacion: obtenerDesagregarRegionalizacion,
            actualizarDesagregarRegionalizacion: actualizarDesagregarRegionalizacion,
            obtenerDatosAdicionales: obtenerDatosAdicionales,
            agregarDatosAdicionales: agregarDatosAdicionales,
            eliminarDatosAdicionales: eliminarDatosAdicionales,
            obtenerResumenCostosVsSolicitado: obtenerResumenCostosVsSolicitado,
            obtenerFuentesProgramarSolicitado: obtenerFuentesProgramarSolicitado,
            agregarFuentesProgramarSolicitado: agregarFuentesProgramarSolicitado,
            validarCapitulos: validarCapitulos,
            obtenerErroresProyecto: obtenerErroresProyecto,
            obtenerFocalizacionPoliticasTransversalesFuentes: obtenerFocalizacionPoliticasTransversalesFuentes,
            guardarFuentesFinanciacionRecursosAjustes: guardarFuentesFinanciacionRecursosAjustes,
            obtenerPoliticasTransversalesCrucePoliticas: obtenerPoliticasTransversalesCrucePoliticas,
            actualizarPoliticasTransversalesCrucePoliticas: actualizarPoliticasTransversalesCrucePoliticas,
            obtenerCuestionarioPreguntas: obtenerCuestionarioPreguntas,
            ObtenerListaTiposRecursosxEntidad: ObtenerListaTiposRecursosxEntidad,
            GuardarPreguntasAprobacionRolPresupuesto: GuardarPreguntasAprobacionRolPresupuesto,
            obtenerCuestionarioPreguntasJefePlaneacion: obtenerCuestionarioPreguntasJefePlaneacion,
            GuardarPreguntasAprobacionRolJefePlaneacion: GuardarPreguntasAprobacionRolJefePlaneacion,
            ObtenerSeccionCapitulo: ObtenerSeccionCapitulo,
            guardarCambiosFirme: guardarCambiosFirme,
            eliminarCapitulosModificados: eliminarCapitulosModificados

        };

        function obtenerDatosGeneralesProyecto(ProyectoId, NivelId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosGeneralesProyecto;
            var params = {
                'ProyectoId': ProyectoId,
                'NivelId': NivelId,
            };
            return $http.get(url, { params });
        }

        function obtenerLocalizacionProyecto(Bpin) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaLocalizaciones + "?bpin=" + Bpin);
        }

        function obtenerTokenMGA(parametros, tipoUsuario) {
            return $http.get(`${apiBackboneServicioBaseUri}${constantesBackbone.apiObtenerTokenMGA}?bpin=${parametros}&tipoUsuario=${tipoUsuario}`);
        }

        function obtenerListaEtapas(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEtapas;
            return $http.post(url, parametros);
        }

        function obtenerListaTipoEntidad(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoTipoEntidad;
            return $http.post(url, parametros);
        }

        function obtenerListaEntidades(peticionobtenerProyectos, idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidadesTotal;
            return $http.post(url, peticionobtenerProyectos);
        }

        function obtenerListaTiposEntidades(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidades + tipoEntidad;
            return $http.get(url);
        }

        function obtenerSectores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaSectoresEntity;
            return $http.post(url, parametros);
        }

        function obtenerListaRecursos(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaTiposRecursos;
            return $http.post(url, peticion);
        }

        function ObtenerListaTiposRecursosxEntidad(peticion, idEntitytype) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTiposRecursosxEntidad + "?entityTypeCatalogId=" + idEntitytype ;
            return $http.post(url, peticion);
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

        function obtenerFuentesFinanciacionProyecto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionConsultar + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function obtenerFuentesFinanciacionVigenciaProyecto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionVigenciaConsultar + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function agregarFuentesFinanciacionProyecto(parametros, usuarioDNP, idFormulario, idInstancia, idAccion, idAplicacion) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionAgregar + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function eliminarFuentesFinanciacionProyecto(fuenteId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionEliminar + "?fuenteId=" + fuenteId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function obtenerDesagregarRegionalizacion(Bpin) {            
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDesagregarRegionalizacionSGP + "?bpin=" + Bpin);
        }

        function actualizarDesagregarRegionalizacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDesagregarRegionalizacionActualizar;
            return $http.post(url, parametros);
        }

        function obtenerDatosAdicionales(fuenteId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesConsultar + "?fuenteId=" + fuenteId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function agregarDatosAdicionales(parametros, usuarioDNP, idFormulario, idInstancia, idAccion, idAplicacion) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesAgregar + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function eliminarDatosAdicionales(cofinanciadorId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesEliminar + "?cofinanciadorId=" + cofinanciadorId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function obtenerResumenCostosVsSolicitado(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneResumenCostosVsSolicitado + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function obtenerFuentesProgramarSolicitado(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuentesProgramarSolicitado + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        
        function agregarFuentesProgramarSolicitado(parametros, usuarioDNP) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarFuentesProgramarSolicitado + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function obtenerFocalizacionPoliticasTransversalesFuentes(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerFocalizacionPoliticasTransversalesFuentes + "?bpin=" + bpin;
            return $http.get(url);
        }

        function validarCapitulos(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneValidarCapitulos;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function obtenerErroresProyecto(guiMacroproceso, idProyecto, idInstancia) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresProyecto;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idProyecto=" + idProyecto;
            url += "&idInstancia=" + idInstancia;
            return $http.get(url);
        }

        function guardarFuentesFinanciacionRecursosAjustes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarFuentesFinanciacionRecursosAjustes + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function obtenerPoliticasTransversalesCrucePoliticas(bpin,idFuente) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesCrucePoliticas;
            url += "?bpin=" + bpin;
            url += "&idfuente=" + idFuente;
            return $http.get(url);
        }

        function actualizarPoliticasTransversalesCrucePoliticas(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizarPoliticasTransversalesCrucePoliticas + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function obtenerCuestionarioPreguntas(idTramite, proyectoId, tipoTramiteId, idNivel, idFormulario, idInstancia) {
            var params = {
                'tramiteId': idTramite,
                'proyectoId': proyectoId,
                'tipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
                'nivelId': idNivel,
                'idInstancia': idInstancia,
                'usuarioDNP': usuarioDNP,
                'tokenAutorizacion': idFormulario
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPreguntasAprobacionRol;
            return $http.get(url, { params });
        }

        function GuardarPreguntasAprobacionRolPresupuesto(parametros, usuarioDNP, idFormulario) {
            //var params = {
            //    'tramiteId': idTramite,
            //    'proyectoId': proyectoId,
            //    'tipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
            //    'nivelId': idNivel,
            //    'usuarioDNP': usuarioDNP,
            //    'tokenAutorizacion': idFormulario
            //};
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarPreguntasAprobacionRol + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function obtenerCuestionarioPreguntasJefePlaneacion(idTramite, proyectoId, tipoTramiteId, idNivel, idFormulario) {
            var params = {
                'tramiteId': idTramite,
                'proyectoId': proyectoId,
                'tipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
                'nivelId': idNivel,
                'usuarioDNP': usuarioDNP,
                'tokenAutorizacion': idFormulario
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPreguntasAprobacionJefe;
            return $http.get(url, { params });
        }

        function GuardarPreguntasAprobacionRolJefePlaneacion(parametros, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarPreguntasAprobacionJefe + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, idUsuario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerSeccionCapitulo;
            url += "?FaseGuid=" + FaseGuid;
            url += "&Capitulo=" + Capitulo;
            url += "&Seccion=" + Seccion;
            url += "&idUsuario=" + idUsuario;
            return $http.get(url);
        }

        function guardarCambiosFirme(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCambiosFirmeRelacionPlanificacion;
            return $http.post(url, data);
        }

        function eliminarCapitulosModificados(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonEliminarCambiosFirme;
            return $http.post(url, data);
        }

    }

})();
