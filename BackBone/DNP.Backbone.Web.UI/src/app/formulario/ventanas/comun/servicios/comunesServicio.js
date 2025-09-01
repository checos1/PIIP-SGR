(function () {
    'use strict';
    angular.module('backbone').factory('comunesServicio', comunesServicio);

    comunesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    var proyectoCargado = 0;
    var bpinCargado = "";

    function comunesServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerListaDireciones: obtenerListaDireciones,
            obtenerSubDireccionTecnica: obtenerSubDireccionTecnica,
            obtenerAnalistasSubDireccionTecnica: obtenerAnalistasSubDireccionTecnica,
            crearReasignacionRadicadOrfeo: crearReasignacionRadicadOrfeo,
            reasignarRadicadoORFEO: reasignarRadicadoORFEO,
            registrarPermisosAccionPorUsuario: registrarPermisosAccionPorUsuario,
            obtenerDetallesTramite: obtenerDetallesTramite,
            solicitarConcepto: solicitarConcepto,
            cargarSolicitudesConcepto: cargarSolicitudesConcepto,
            obtenerConceptoDireccionTecnicaTramite: obtenerConceptoDireccionTecnicaTramite,
            enviarConceptoDireccionTecnicaTramite: enviarConceptoDireccionTecnicaTramite,
            eliminarPermisos: eliminarPermisos,
            guardarConceptoDireccionTecnicaTramite: guardarConceptoDireccionTecnicaTramite,
            obtenerDatosConceptoPorInstancia: obtenerDatosConceptoPorInstancia,
            obtenerInstanciaProyectoTramite: obtenerInstanciaProyectoTramite,
            obtenerInstanciasActivasProyectos: obtenerInstanciasActivasProyectos,
            ActualizarInstanciaProyecto: ActualizarInstanciaProyecto,
            obtenerEntidadAsociarProyecto: obtenerEntidadAsociarProyecto,
            obtenerContraCreditos: obtenerContraCreditos,
            obtenerCreditos: obtenerCreditos,
            guardarProyectos: guardarProyectos,
            guardarMontosTramite: guardarMontosTramite,
            obtenerListaProyectosFuentes: obtenerListaProyectosFuentes,
            actualizarTramitesFuentesPresupuestales: actualizarTramitesFuentesPresupuestales,
            ObtenerProyectoAsociacion: ObtenerProyectoAsociacion,
            asociarProyecto: asociarProyecto,
            obtenerPreguntasJustificacion: obtenerPreguntasJustificacion,
            obtenerPreguntasProyectoActualizacionPaso: obtenerPreguntasProyectoActualizacionPaso,
            obtenerPreguntastramiteProyectos: obtenerPreguntastramiteProyectos,
            guardarRespuestasJustificacion: guardarRespuestasJustificacion,
            obtenerPasoAjuste: obtenerPasoAjuste,
            guardarFuentesTramiteProyectoAprobacion: guardarFuentesTramiteProyectoAprobacion,
            guardarCambiosFirme: guardarCambiosFirme,
            obtenerDatosProyectosPorTramite: obtenerDatosProyectosPorTramite,
            obtenerListaProyectosFuentesAprobada: obtenerListaProyectosFuentesAprobada,
            obtenerProyectosTramite: obtenerProyectosTramite,
            obtenerEntidadTramite: obtenerEntidadTramite,
            obtenerCalendarioPeriodo: obtenerCalendarioPeriodo,
            getOrigenRecursosTramite: getOrigenRecursosTramite,
            setOrigenRecursosTramite: setOrigenRecursosTramite,
            obtenerTramitesDistribucionAnteriores: obtenerTramitesDistribucionAnteriores,
            ObtenerPresupuestalProyectosAsociados: ObtenerPresupuestalProyectosAsociados,
            ObtenerPresupuestalProyectosAsociados_Adicion: ObtenerPresupuestalProyectosAsociados_Adicion,
            ObtenerResumenReprogramacionPorVigencia: ObtenerResumenReprogramacionPorVigencia,
            GuardarDatosReprogramacion: GuardarDatosReprogramacion,
            notificarUsuariosPorInstanciaPadre: notificarUsuariosPorInstanciaPadre,
            guardarReprogramacionPorProductoVigencia: guardarReprogramacionPorProductoVigencia,
            obtenerResumenReprogramacionPorProductoVigencia: obtenerResumenReprogramacionPorProductoVigencia,
            ObtenerDatosProgramacionEncabezado: ObtenerDatosProgramacionEncabezado,
            ObtenerDatosProgramacionDetalle: ObtenerDatosProgramacionDetalle,
            GuardarDatosProgramacionDistribucion: GuardarDatosProgramacionDistribucion,
            GuardarDatosProgramacionFuentes: GuardarDatosProgramacionFuentes,
            GuardarDatosProgramacionIniciativa: GuardarDatosProgramacionIniciativa,
            ObtenerTablasBasicas: ObtenerTablasBasicas,
            GuardarProgramacionRegionalizacion: GuardarProgramacionRegionalizacion,
            consultarPoliticasTransversalesProgramacion: consultarPoliticasTransversalesProgramacion,
            agregarPoliticasTransversalesProgramacion: agregarPoliticasTransversalesProgramacion,
            consultarPoliticasTransversalesCategoriasProgramacion: consultarPoliticasTransversalesCategoriasProgramacion,
            eliminarPoliticasProyectoProgramacion: eliminarPoliticasProyectoProgramacion,
            agregarCategoriasPoliticaTransversalesProgramacion: agregarCategoriasPoliticaTransversalesProgramacion,
            guardarPoliticasTransversalesCategoriasProgramacion: guardarPoliticasTransversalesCategoriasProgramacion,
            eliminarCategoriasProyectoProgramacion: eliminarCategoriasProyectoProgramacion,
            eliminarCategoriaPoliticasProyectoProgramacion: eliminarCategoriaPoliticasProyectoProgramacion,
            obtenerCrucePoliticasProgramacion: obtenerCrucePoliticasProgramacion,
            politicasSolicitudConceptoProgramacion: politicasSolicitudConceptoProgramacion,
            guardarCrucePoliticasProgramacion: guardarCrucePoliticasProgramacion,
            solicitarConceptoDTProgramacion: solicitarConceptoDTProgramacion,
            obtenerResumenSolicitudConceptoProgramacion: obtenerResumenSolicitudConceptoProgramacion,
            validarCalendarioProgramacion: validarCalendarioProgramacion,
            consultarPoliticasTransversalesAprobacionesModificaciones: consultarPoliticasTransversalesAprobacionesModificaciones,
            ObtenerDatosProgramacionProducto: ObtenerDatosProgramacionProducto,
            GuardarDatosProgramacionProducto: GuardarDatosProgramacionProducto,
            obtenerDepartamentos: obtenerDepartamentos,
            obtenerMunicipios: obtenerMunicipios,
            obtenerAgrupaciones: obtenerAgrupaciones,
            obtenerTipoAgrupaciones: obtenerTipoAgrupaciones,
            obtenerAgrupacionesCompleta: obtenerAgrupacionesCompleta,
            ObtenerLocalizacionProyecto: obtenerLocalizacionProyecto,
            guardarLocalizacion: guardarLocalizacion,
            setProyectoCargado: setProyectoCargado,
            getProyectoCargado: getProyectoCargado,
            setBpinCargado: setBpinCargado,
            getBpinCargado: getBpinCargado,           
            GuardarDatosInclusion: GuardarDatosInclusion,


        };

        function setProyectoCargado(dato) {
            proyectoCargado = dato;
        }

        function getProyectoCargado(peticion) {
            return proyectoCargado;
        }

        function setBpinCargado(dato) {
            bpinCargado = dato;
        }

        function getBpinCargado() {
            return bpinCargado;
        }

        function obtenerListaDireciones(peticion) {
            var metodo = constantesBackbone.apiBackboneObtenerListaDirecciones + '?idEntididad=' + peticion.IdFiltro;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function obtenerSubDireccionTecnica(peticion) {
            var metodo = constantesBackbone.apiBackboneObtenerListaSubdirecciones + '?idEntididadType=' + peticion.IdFiltro;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function obtenerAnalistasSubDireccionTecnica(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAnalistasSubDireccionTecnica, peticion);
        }

        function crearReasignacionRadicadOrfeo(usuarioIdOrigen, usuarioIdDestino, tramiteId) {
            const UsuarioOrigen = {
                Login: usuarioIdOrigen
            };

            const UsuarioDestino = {
                Login: usuarioIdDestino
            };

            const ReasignacionRadicadoDto = {
                UsuarioOrigen: UsuarioOrigen,
                UsuarioDestino: UsuarioDestino,
                TramiteId: tramiteId,
            };

            reasignarRadicadoORFEO(ReasignacionRadicadoDto).then(function (response) { }, (err) => {
                swal('', "Error al realizar la reasignación", 'error');
            });
        }


        function reasignarRadicadoORFEO(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneReasignarRadicado;
            return $http.post(url, parametros);
        }

        function registrarPermisosAccionPorUsuario(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneFlujoRegistrarPermisosAccionPorUsuario;
            return $http.post(url, parametros);
        }

        function obtenerDetallesTramite(numerotramite) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerDetallesTramite}` + '?numeroTramite=' + numerotramite;
            return $http.get(url);
        }

        function solicitarConcepto(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSolicitarConcepto, peticion);
        }

        function cargarSolicitudesConcepto(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneCargarSolicitudesConcepto}` + '?tramiteId=' + tramiteId;
            return $http.post(url);
        }

        function obtenerConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConceptoDireccionTecnicaTramite, peticion);
        }

        function enviarConceptoDireccionTecnicaTramite(tramiteId, usuarioDnp) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneEnviarConceptoDireccionTecnicaTramite}`;
            url = url + '?tramiteId=' + tramiteId + '&usuarioDnp=' + usuarioDnp;
            return $http.post(url);
        }

        function eliminarPermisos(usuarioDestino, tramiteId, aliasNivel) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneEliminarPermisosAccionesUsuarios}`;
            url = url + '?usuarioDestino=' + usuarioDestino + '&tramiteId=' + tramiteId + '&aliasNivel=' + aliasNivel + '&InstanciaId=' + '00000000-0000-0000-0000-000000000000' ;
            return $http.post(url);
        }

        function guardarConceptoDireccionTecnicaTramite(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarConceptoDireccionTecnicaTramite, peticion);
        }

        function obtenerDatosConceptoPorInstancia(instanciaId) {
            var metodo = constantesBackbone.apiBackboneObtenerDatosProyectoConceptoPorInstancia + '?instanciaId=' + instanciaId;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function obtenerInstanciaProyectoTramite(InstanciaId, BPIN) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInstanciaProyectoTramite;
            url += "?InstanciaId=" + InstanciaId + "&BPIN=" + BPIN;
            return $http.get(url);
        }

        function obtenerInstanciasActivasProyectos(Bpins) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerInstanciasActivasProyectos}` + '?Bpins=' + Bpins;
            return $http.get(url);
        }

        function ActualizarInstanciaProyecto(proyectoTramiteDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarInstanciaProyecto;
            return $http.post(url, proyectoTramiteDto);
        }

        function obtenerEntidadAsociarProyecto(instanciaId, acciontramiteProyecto) {
            var metodo = constantesBackbone.apiBackboneobtenerEntidadAsociarProyecto + '?InstanciaId=' + instanciaId + "&AccionTramiteProyecto=" + acciontramiteProyecto;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function obtenerContraCreditos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerContraCreditos;
            return $http.post(url, parametros);
        }
        function obtenerCreditos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCredito;
            return $http.post(url, parametros);
        }

        function guardarProyectos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarProyectosTramiteNegocio;
            return $http.post(url, parametros);
        }

        function guardarMontosTramite(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarMontosTramite;
            return $http.post(url, parametros);
        }

        function obtenerListaProyectosFuentes(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaProyectosFuentes;
            url = url + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function actualizarTramitesFuentesPresupuestales(TramiteFuentesPresupuestalesDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarTramitesFuentesPresupuestales;
            return $http.post(url, TramiteFuentesPresupuestalesDto);
        }

        function ObtenerProyectoAsociacion(bpin, tramiteid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoAsociacionVFO;
            url += "?bpin=" + bpin;
            url += "&tramiteid=" + tramiteid;
            url += "&tipoTramite=AL";
            return $http.get(url);
        }

        function asociarProyecto(asociarProyectoDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAsociarProyectoVFOPLN;
            return $http.post(url, asociarProyectoDto);
        }

        function obtenerPreguntasJustificacion(idTramite, proyectoId, tipoTramiteId, tipoRolId, idNivel) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasJustificacion;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId === null ? 0 : tipoTramiteId,
                'TipoRolId': tipoRolId,
                'IdNivel': idNivel
            };
            return $http.get(url, { params });
        }

        function obtenerPreguntasProyectoActualizacionPaso(idTramite, proyectoId, tipoTramiteId, idNivel, tipoRolId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasProyectoActualizacionPaso;
            var params = {
                'TramiteId': idTramite,
                'ProyectoId': proyectoId,
                'TipoTramiteId': tipoTramiteId == null ? 0 : tipoTramiteId,
                'IdNivel': idNivel,
                'TipoRolId': tipoRolId,
            };
            return $http.get(url, { params });
        }

        function obtenerPreguntastramiteProyectos(idTramite,  tipoTramiteId, idNivel, tipoRolId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasJustificacionPorProyectos;
            var params = {
                'TramiteId': idTramite,
                'TipoTramiteId': tipoTramiteId == null ? 0 : tipoTramiteId,
                'IdNivel': idNivel,
                'TipoRolId': tipoRolId,
            };
            return $http.get(url, { params });
            
        }

        function guardarRespuestasJustificacion(parametros) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarRespuestasJustificacion, parametros);
        }

        function obtenerPasoAjuste(tramiteId, proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneTramiteAjusteEnPasoUno;
            url = url + '?tramiteId=' + tramiteId + '&proyectoId=' + proyectoId;
            return $http.get(url);
        }

        function guardarFuentesTramiteProyectoAprobacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarFuentesTramiteProyectoAprobacion;
            return $http.post(url, parametros);
        }

        function guardarCambiosFirme(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCambiosFirmeRelacionPlanificacion;
            return $http.post(url, data);
        }

        function obtenerDatosProyectosPorTramite(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProyectosPorTramite;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function obtenerListaProyectosFuentesAprobada(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaProyectosFuentesAprobado;
            url = url + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function obtenerProyectosTramite(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectosTramiteNegocio}${tramiteId}`;
            return $http.get(url);
        }

        function obtenerEntidadTramite(numeroTramite) {
            var metodo = constantesBackbone.apiBackboneObtenerEntidadTramite + '?numeroTramite=' + numeroTramite;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }
        
        function obtenerCalendarioPeriodo(bpin) {
            if (bpin === undefined)
                bpin = '0';
            var metodo = constantesBackbone.apiBackboneObtenerCalendarioPeriodo + '?bpin=' + bpin;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function getOrigenRecursosTramite(tramiteId) {
            var metodo = constantesBackbone.apiBackboneGetOrigenRecursosTramite + '?tramiteId=' + tramiteId;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function getOrigenRecursosTramite(tramiteId) {
            var metodo = constantesBackbone.apiBackboneGetOrigenRecursosTramite + '?tramiteId=' + tramiteId;
            return $http.get(apiBackboneServicioBaseUri + metodo);
        }

        function setOrigenRecursosTramite(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSetOrigenRecursosTramite;
            return $http.post(url, data);
        }

        function obtenerTramitesDistribucionAnteriores(instanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramitesDistribucionAnteriores + "?instanciaId=" + instanciaId;
            return $http.get(url);
        }

        function ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPresupuestalProyectosAsociados + "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId;
            return $http.get(url);
        }

        function ObtenerPresupuestalProyectosAsociados_Adicion(TramiteId, InstanciaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPresupuestalProyectosAsociados_Adicion + "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId;
            return $http.get(url);
        }

        function ObtenerResumenReprogramacionPorVigencia(InstanciaId, ProyectoId, TramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenReprogramacionPorVigencia + "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId + "&ProyectoId=" + ProyectoId;
            return $http.get(url);
        }

        function GuardarDatosReprogramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarDatosReprogramacion;
            return $http.post(url, parametros);
        }

        function notificarUsuariosPorInstanciaPadre(instanciaId, nombreNotificacion, texto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneNotificarUsuariosPorInstanciaPadre + '?instanciaId=' + instanciaId + '&nombreNotificacion=' + nombreNotificacion + '&texto=' + texto;
            return $http.post(url);
        }

        function guardarReprogramacionPorProductoVigencia(reprogramacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarReprogramacionPorProductoVigencia;
            return $http.post(url, reprogramacionValores);
        }

        function obtenerResumenReprogramacionPorProductoVigencia(instanciaId, proyectoId, tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarResumenReprogramacionPorProductoVigencia + "?InstanciaId=" + instanciaId + "&TramiteId=" + tramiteId + "&ProyectoId=" + proyectoId;
            return $http.get(url);
        }

        function ObtenerDatosProgramacionEncabezado(EntidadDestinoId, TramiteId, origen) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProgramacionEncabezado + "?EntidadDestinoId=" + EntidadDestinoId + "&TramiteId=" + TramiteId + "&origen=" + origen;
            return $http.get(url);
        }

        function ObtenerDatosProgramacionDetalle(TramiteProyectoId, origen) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProgramacionDetalle + "?TramiteProyectoId=" + TramiteProyectoId + "&origen=" + origen;
            return $http.get(url);
        }

        function GuardarDatosProgramacionDistribucion(programacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarDatosProgramacionDistribucion;
            return $http.post(url, programacionValores);
        }

        function GuardarDatosProgramacionFuentes(programacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarDatosProgramacionFuentes;
            return $http.post(url, programacionValores);
        }

        function GuardarDatosProgramacionIniciativa(programacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarDatosProgramacionIniciativa;
            return $http.post(url, programacionValores);
        }

        function ObtenerTablasBasicas(jsonCondicion, Tabla) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerTablasBasicas + "?jsonCondicion=" + jsonCondicion + "&Tabla=" + Tabla;
            return $http.get(url);
        }

        //Regionalizacion
        function GuardarProgramacionRegionalizacion(programacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarProgramacionRegionalizacion;
            return $http.post(url, programacionValores);
        }
        //Politicas Transversales
        function consultarPoliticasTransversalesProgramacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasTransversalesProgramacion + "?bpin=" + Bpin;
            return $http.get(url);
        }
        function agregarPoliticasTransversalesProgramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneAgregarPoliticasTransversalesProgramacion;
            return $http.post(url, parametros);
        }
        function consultarPoliticasTransversalesCategoriasProgramacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasTransversalesCategoriasProgramacion + "?bpin=" + Bpin;
            return $http.get(url);
        }
        function eliminarPoliticasProyectoProgramacion(tramiteidProyectoId, politicaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarPoliticasProyectoProgramacion + "?tramiteidProyectoId=" + tramiteidProyectoId + "&politicaId=" + politicaId;
            return $http.post(url);
        }
        function agregarCategoriasPoliticaTransversalesProgramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneAgregarCategoriasPoliticaTransversalesProgramacion;
            return $http.post(url, parametros);
        }
        function guardarPoliticasTransversalesCategoriasProgramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarPoliticasTransversalesCategoriasProgramacion;
            return $http.post(url, parametros);
        }
        function eliminarCategoriasProyectoProgramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarCategoriasProyectoProgramacion;
            return $http.post(url, parametros);
        }
        function eliminarCategoriaPoliticasProyectoProgramacion(proyectoId, politicaId, categoriaId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarPoliticasProyectoProgramacion + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId;
            return $http.post(url);
        }
        function obtenerCrucePoliticasProgramacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerCrucePoliticasProgramacion + "?bpin=" + Bpin;
            return $http.get(url);
        }
        function politicasSolicitudConceptoProgramacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBonePoliticasSolicitudConceptoProgramacion + "?bpin=" + Bpin;
            return $http.get(url);
        }
        function guardarCrucePoliticasProgramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarCrucePoliticasProgramacion;
            return $http.post(url, parametros);
        }
        function solicitarConceptoDTProgramacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSolicitarConceptoDTProgramacion;
            return $http.post(url, parametros);
        }
        function obtenerResumenSolicitudConceptoProgramacion(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerResumenSolicitudConceptoProgramacion + "?bpin=" + Bpin;
            return $http.get(url);
        }

        function validarCalendarioProgramacion(entityTypeCatalogOptionId, nivelId, seccionCapituloId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneValidarCalendarioProgramacion;
            url += "?entityTypeCatalogOptionId=" + entityTypeCatalogOptionId;
            url += "&nivelId=" + nivelId;
            url += "&seccionCapituloId=" + seccionCapituloId;
            return $http.get(url);
        }

        function consultarPoliticasTransversalesAprobacionesModificaciones(Bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasTransversalesAprobacionesModificaciones + "?bpin=" + Bpin;
            return $http.get(url);
        }

        //Productos
        function ObtenerDatosProgramacionProducto(TramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProgramacionProducto + "?TramiteId=" + TramiteId;
            return $http.get(url);
        }

        function GuardarDatosProgramacionProducto(programacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarDatosProgramacionProducto;
            return $http.post(url, programacionValores);
        }
        function obtenerDepartamentos() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDepartamentosLocalizacion;
            return $http.post(url);
        }
        function obtenerMunicipios(EntidadesPorCodigoParametrosDto, idDepartamento) {
            var peticion = {
                IdUsuario: usuarioDNP,
                //IdObjeto: $sessionStorage.idProyecto,
                IdObjeto: 0,
                Aplicacion: null,
                ListaIdsRoles: null,
                idDepartamento: idDepartamento
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMunicipiosLocalizacion;
            return $http.post(url, peticion);
        }
        function obtenerAgrupaciones(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaAgrupaciones;
            return $http.post(url, peticion);
        }
        function obtenerTipoAgrupaciones(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTipoAgrupaciones;
            return $http.post(url, peticion);
        }
        function obtenerAgrupacionesCompleta(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaAgrupacionesCompleta;
            return $http.post(url, peticion);
        }
        function obtenerLocalizacionProyecto(Bpin) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaLocalizaciones + "?bpin=" + Bpin);
        }
        function guardarLocalizacion(LocalizacionProyectoAjusteDto, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarLocalizacion + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, LocalizacionProyectoAjusteDto);
        }
        function GuardarDatosInclusion(programacionValores) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarDatosInclusion;
            return $http.post(url, programacionValores);
        }
    }
})();