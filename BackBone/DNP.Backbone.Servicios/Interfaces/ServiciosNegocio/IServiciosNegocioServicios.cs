namespace DNP.Backbone.Servicios.Interfaces.ServiciosNegocio
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.Backbone.Dominio.Dto.Acciones;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;
    using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;

    public interface IServiciosNegocioServicios
    {
        Task<List<Dominio.Dto.ProyectoDto>> ObtenerListaProyectoPorTramite(ParametrosProyectosDto parametros);
        Task<List<CatalogoDto>> ObtenerListaCatalogo(ProyectoParametrosDto peticionObtenerProyecto, CatalogoEnum catalogoEnum);
        Task<string> ObtenerCatalogoReferencia(EntidadesPorCodigoParametrosDto peticion, string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia);
        Task<string> ObtenerListaCatalogoEntidades(ProyectoParametrosDto peticion, CatalogoEnum catalogoEnum);
        Task<List<EntidadCatalogoSTDto>> ObtenerListaCatalogoDT(ProyectoParametrosDto peticionObtenerProyecto, CatalogoEnum catalogoEnum);
        Task<List<EstadoDto>> ObtenerListaEstado(ProyectoParametrosDto peticion);
        Task<List<ConfiguracionUnidadMatrizDTO>> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion);
        Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion, string idUsuarioDNP);
        Task<List<ProyectosEntidadesDto>> ObtenerProyectos(ParametrosProyectosDto dto, string idUsuarioDnp);
        Task<string> ObtenerProyectoListaLocalizaciones(string bpin, string IdUsuario, string tokenAutorizacion);
        Task<object> InsertAuditoriaEntidadProyecto(Dominio.Dto.Proyecto.AuditoriaEntidadDto auditoriaEntidad, string idUsuarioDNP);
        Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP);
        Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocio(Dominio.Dto.Proyecto.DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<Dominio.Dto.Proyecto.ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string tokenAutorizacion);

        Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramite(int TipoTramiteId, string Rol, int tramiteId, string usuarioDnpo, string tokenAutorizacion, string nivelId);
        Task<List<FuentePresupuestalDto>> ObtenerFuentesInformacionPresupuestal(string usuarioDNP);

        Task<List<ProyectoFuentePresupuestalDto>> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto, string usuarioDNP);

        Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, string usuarioDNP, bool isCDP);

        Task<RespuestaGeneralDto> ActualizarInstanciaProyecto(Dominio.Dto.Proyecto.ProyectosTramiteDto parametros, string usuarioDnp);
        Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string IdNivel, string tokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> parametros, string usuarioDnp);

        Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp);



        Task<RespuestaGeneralDto> ActualizarValoresProyecto(Dominio.Dto.Proyecto.ProyectosTramiteDto parametros, string usuarioDnp);

        Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocio(Dominio.Dto.Proyecto.DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<List<JustificacionTematicaDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp, string tokenAutorizacion);
        Task<List<Dominio.Dto.Proyecto.ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId, string usuarioDnpo, string tokenAutorizacion);

        Task<List<TipoRequisitoDto>> ObtenerTiposRequisito(string usuarioDnp);
        Task<List<Dominio.Dto.Proyecto.FuentesTramiteProyectoAprobacionDto>> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto, string usuarioDnpo, string tokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<Dominio.Dto.Proyecto.FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp);
        Task<ResponseDto<EnvioSubDireccionDto>> SolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto);
        Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto);

        Task<CodigoPresupuestalDto> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp);

        Task<TramiteValoresProyectoDto> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuarioDnp);
        Task<List<TramitesProyectosDto>> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int tramiteId, string usuarioDnp);
        Task<CapituloModificado> ObtenerCapitulosModificadosCapitoSeccion(string guiMacroproceso, int idProyecto, Guid idInstancia, string capitulo, string seccion, string usuarioDNP);
        Task<RespuestaGeneralDto> DevolverProyecto(DevolverProyectoDto parametros, string usuarioDnp);
        Task<List<ConceptoDireccionTecnicaTramiteDto>> ObtenerConceptoDireccionTecnicaTramite(int tramiteId1, Guid nivelid, string usuario);
        Task<RespuestaGeneralDto> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramiteDto> parametros, string usuario);
        RespuestaDocumentoCONPES ObtenerProyectoConpes(string conpes, string idUsuario);
        Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocioAprobacion(Dominio.Dto.Proyecto.DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<PlantillaCarta> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite, string usuarioDnp);
        Task<List<Carta>> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId, string usuarioDnp);
        Task<string> ObtenerCartaConceptoDatosDespedida(int tramiteId, string IdUsuario, string tokenAutorizacion);
        Task<RespuestaGeneralDto> ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros, string usuarioDnp);
        Task<UsuarioTramite> VerificaUsuarioDestinatario(UsuarioTramite parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarCartaDatosIniciales(Carta parametros, string usuarioDnp);
        Task<List<UsuarioTramite>> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite, string usuarioDnp);
        Task<EncabezadoGeneralDto> ObtenerEncabezadoGeneral(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp);
        Task<EncabezadoSGRDto> ObtenerEncabezadoSGR(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp);
        Task<EncabezadoSGPDto> ObtenerEncabezadoSGP(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp);
        Task<CapituloConpes> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string idUsuario, string NivelId, string FlujoId);
        Task<string> ObtenerDesagregarRegionalizacion(string bpin, string IdUsuario, string tokenAutorizacion);
        Task<RespuestaGeneralDto> ActualizarDesagregarRegionalizacion(Dominio.Dto.Proyecto.DesagregarRegionalizacionDto parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string idUsuario);
        Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string idUsuario);
        Task<RespuestaGeneralDto> Firmar(int tramiteId, string radicadoSalida, string idUsuario);
        Task<List<SeccionCapituloDto>> SeccionesCapitulosModificadosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia, string idUsuario);
        Task<List<SeccionCapituloDto>> SeccionesCapitulosByMacroproceso(string guiMacroproceso, string idUsuario, string NivelId, string FlujoId);
        Task<List<RelacionPlanificacionDto>> CambiosRelacionPlanificacion(int IdProyecto, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarCambiosRelacionPlanificacion(CapituloModificado parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes conpes, string idUsuario);
        Task<List<DocumentoCONPES>> EliminarProyectoConpes(string proyectoid, string conpesid, string idUsuario);
        Task<List<CuerpoConceptoCDP>> ObtenerCuerpoConceptoCDP(int tramiteId, string usuarioDnp);
        Task<List<CuerpoConceptoAutorizacion>> ObtenerCuerpoConceptoAutorizacion(int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> ValidarSeccionesCapitulosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp);
        Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp);
        Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp);
        Task<List<ErroresTramiteDto>> ObtenerErroresViabilidad(string guiMacroproceso, int IdProyecto, string IdNivel, string IdInstancia, string usuarioDnp);
        Task<Carta> ConsultarCarta(int tramiteId, string usuarioDnp);
        Task<string> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario);
        Task<ResponseDto<bool>> CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp);
        Task<ResponseDto<bool>> ConsultarRadicado(string radicado, string usuarioDnp);
        Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto radicado, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarCambiosJustificacionHorizonte(CapituloModificado parametros, string usuarioDnp);
        Task<List<JustificacionHorizontenDto>> ObtenerJustificacionHorizonte(int IdProyecto, string usuarioDnp);
        Task<int> TramiteEnPasoUno(Guid InstanciaId, string usuarioDnp);
        Task<string> ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin, string usuarioDnp, string tokenAutorizacion);
        Task<string> ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuarioDNP);
        Task<ResponseDto<List<TramiteConpesDetailDto>>> ObtenerConpesTramite(int tramiteId, string usuarioDnp);
        Task<ResponseDto<bool>> AsociarConpesTramite(AsociarConpesTramiteRequestDto model, string usuarioDnp);

        Task<ResponseDto<bool>> RemoverAsociacionConpesTramite(RemoverAsociacionConpesTramiteDto model, string usuarioDnp);

        Task<ResponseDto<PeriodoPresidencialDto>> ObtenerPeriodoPresidencial(string usuarioDnp);
        Task<List<ErroresTramiteDto>> ObtenerErroresTramite(string guiMacroproceso, string IdInstancia, string accionid, string usuarioDnp, bool tieneCDP);
        Task<ResultadoProcedimientoDto> guardarLocalizacion(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion);
        Task<List<DepartamentoCatalogoDto>> obtenerDepartamento(string usuarioDNP);
        Task<List<AgrupacionCodeDto>> ConsultarAgrupacionesCompleta(string usuarioDNP);

        Task<IHttpActionResult> obtenerMunicipio(ProyectoParametrosDto peticion);
        Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuarioDnp);

        Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string IdMacroproceso, string IdInstancia, string FaseId, string usuarioDnp);
        Task<List<int?>> ObtenerListaVigenciasProyecto(ProyectoParametrosDto peticion);
        Task<List<TramiteProyectoVFODto>> ObtenerProyectoAsociacionVFO(string bpin, int tramite, string tipoTramite, string usuarioDnp);

        Task<string> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto, string usuarioDnp);



        Task<SeccionCapituloDto> ObtenerSeccionCapitulo(string faseGuid, string capitulo, string seccion, string idUsuario, string NIvelid, string FlujoId);

        Task<string> ObtenerPoliticasTransversalesCrucePoliticas(string Bpin, int IdFuente, string IdUsuario, string tokenAutorizacion);

        Task<RespuestaGeneralDto> ActualizarPoliticasTransversalesCrucePoliticas(PoliticasTCrucePoliticasDto parametros, string usuarioDnp);

        Task<dynamic> CrearRadicadoEntradaTramite(int tramiteId, string usuarioDnp);

        Task<dynamic> CerrarRadicadosTramite(string numeroTramite, string usuarioDnp);

        Task<DatosProyectoTramiteDto> ObtenerDatosProyectoTramite(int tramiteId, string usuarioDnp);
        Task<List<DatosProyectoTramiteDto>> ObtenerDatosProyectosPorTramite(int tramiteId, string usuarioDnp);

        #region Vigencias Futuras

        Task<string> ObtenerDatosCronograma(Guid instanciaId, string usuarioDNP);
        Task<List<JustificacionPasoDto>> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp);

        Task<List<TramiteDeflactoresDto>> ObtenerDeflactores(string usuarioDnp);
        Task<List<TramiteProyectoDto>> ObtenerProyectoTramite(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<string> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto, string usuarioDnp);
        Task<VigenciaFuturaCorrienteDto> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin, string usuarioDnp);
        Task<VigenciaFuturaConstanteDto> ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int tramiteId, string usuarioDnp);

        Task<InformacionPresupuestalValoresDto> ObtenerInformacionPresupuestalValores(int tramiteId, string usuarioDnp);

        Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuarioDnp);
        Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuarioDnp);
        Task<string> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuarioDnp);
        #endregion Vigencias Futuras 

        Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConceptoPorTramite(int tramiteId, string usuarioDnp);

        Task<RespuestaGeneralDto> EliminarCapitulosModificados(CapituloModificado parametros, string usuarioDnp);
        Task<CrearRadicadoResponseDto> CrearRadicadoSalida(RadicadoSalidaRequestDto radicado, string usuarioDnp);
        Task<int> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, string usuarioDnp, Guid InstanciaId = default(Guid));
        Task<AccionDto> ObtenerAccionActualyFinal(int tramiteId, string bpin, string usuarioDnp);
        Task<string> ObtenerTiposRecursosEntidad(ProyectoParametrosDto peticion, int entityTypeCatalogId);

        Task<RespuestaGeneralDto> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuarioDnp, string usuarioLogueado);
        Task<List<TramiteModalidadContratacionDto>> ObtenerModalidadesContratacion(int mostrar, string usuarioDnp);
        Task<ActividadPreContractualDto> ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionIdstring, string usuarioDnp);
        Task<ActividadPreContractualDto> ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades, string usuarioDnp);
        Task<ProductosConstantesVF> ObtenerProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase, string usuarioDnp);
        Task<ProductosCorrientesVF> ObtenerProductosVigenciaFuturaCorriente(string Bpin, int TramiteId, string usuarioDnp);

        Task<string> ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuarioDNP);

        Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId, string usuarioDnp);

        Task<List<DatosUsuarioDto>> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia, string usuarioDNP);
        Task<ModificacionLeyendaDto> ObtenerModificacionLeyenda(int tramiteId, int ProyectoId, string usuarioDnp);
        Task<string> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuarioDnp);
        Task<List<EntidadCatalogoDTDto>> ObtenerListaDirecionesDNP(Guid idEntididad, string usuarioDnp);

        Task<List<EntidadCatalogoDTDto>> ObtenerListaSubdirecionesPorParentId(int idEntididadType, string usuarioDnp);

        Task<RespuestaGeneralDto> BorrarFirma(string idUsuario);

        Task<ProyectosCartaDto> ObtenerProyectosCartaTramite(int TramiteId, string usuarioDnp);
        Task<DetalleCartaConceptoALDto> ObtenerDetalleCartaAL(int TramiteId, string usuarioDnp);
        Task<int> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<DatosProyectoTramiteDto> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId, string usuarioDnp);
        Task<RespuestaGeneralDto> FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada parametros, string idUsuario);
        Task<List<TramiteLiberacionVfDto>> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<VigenciaFuturaResponse> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuarioDnp);
        Task<VigenciaFuturaResponse> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuarioDnp);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp);


        Task<List<EntidadesAsociarComunDto>> obtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto, string usuarioDnp);
        Task<CartaConcepto> ConsultarCartaConcepto(int tramiteId, string usuarioDnp);
        Task<int> ValidacionPeriodoPresidencial(int tramiteId, string usuarioDnp);
        Task<string> GuardarMontosTramite(List<ProyectosEnTramiteDto> proyectosEnTramiteDto, string usuarioDnp);
        Task<List<ProyectoJustificacioneDto>> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel, string usuarioDnp);
        Task<List<tramiteVFAsociarproyecto>> ObtenerTramitesVFparaLiberar(string numTramite, string usuarioDnp);
        Task<string> GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuarioDnp);
        Task<List<ResumenLiberacionVfDto>> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<ValoresUtilizadosLiberacionVfDto> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuariodnp);
        Task<int> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId, string usuarioDnp);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp);
        Task<AlcanceTramiteMGADto> CrearAlcanceTramite(AlcanceTramiteDto alcanceTramite, string usuarioDnp);
        Task<List<TipoMotivoAnulacionDto>> ObtenerTiposMotivoAnulacion(string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarCargueMasivo(ObjetoNegocioDto contenido, string usuarioDnp);
        Task<string> ConsultarCargueExcel(ObjetoNegocioDto contenido, string usuarioDnp);
        Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuarioDnp);
        Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuarioDnp);
        Task<List<EntidadesAsociarComunDto>> ObtenerEntidadTramite(string numeroTramite, string usuarioDnp);
        Task<VigenciaFuturaResponse> EliminarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto eliminarLiberacionVigenciasFuturasDto, string usuarioDnp);
        Task<List<CalendarioPeriodoDto>> ObtenerCalendartioPeriodo(string bpin, string usuarioDnp);
        Task<PresupuestalProyectosAsociadosDto> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId, string usuarioDnp);
        Task<string> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId, string usuarioDnp);
        Task<string> ObtenerResumenReprogramacionPorVigencia(int TramiteId, Guid InstanciaId, int ProyectoId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosReprogramacion(DatosReprogramacionDto Reprogramacion, string usuarioDnp);
        Task<string> PermisosAccionPaso(AccionFlujoDto parametros);
        Task<OrigenRecursosDto> GetOrigenRecursosTramite(int TramiteId, string usuarioDnp);
        Task<VigenciaFuturaResponse> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuarioDnp);
        Task<SystemConfigurationDto> ConsultarSystemConfiguracion(string VariableKey, string Separador, string usuarioDnp);
        Task<string> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId, string usuarioDnp);
        Task<int> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<List<TramiteRVFAutorizacionDto>> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramite, string tipoTramite, string usuarioDnp);
        Task<string> AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuarioDnp);
        Task<TramiteRVFAutorizacionDto> ObtenerAutorizacionAsociada(int tramiteId, string usuarioDnp);
        Task<string> EliminaReprogramacionVF(ReprogramacionDto reprogramacionDto, string usuarioDnp);
        Task<List<ErroresTramiteDto>> ObtenerErroresProgramacion(string IdInstancia, string accionid, string usuarioDnp);

        //TramiteSGP
        Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyectoSGP(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocioSGP(InstanciaTramiteDto instanciaTramiteDto);

        //TramiteSGP - Información Presupuestal
        Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestalSGP(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesSGP(int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacionSGP(List<Dominio.Dto.Proyecto.FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobadoSGP(int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarTramiteTipoRequisitoSGP(List<TramiteRequitoDto> parametros, string usuarioDnp);
        Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramiteSGP(int pProyectoId, int? pTramiteId, string usuarioDNP, bool isCDP);

        Task<List<Dominio.Dto.Proyecto.ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioSGP(int TramiteId, string usuarioDnpo, string tokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocioSGP(Dominio.Dto.Proyecto.DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<string> ValidacionProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string tokenAutorizacion);
    }
}
