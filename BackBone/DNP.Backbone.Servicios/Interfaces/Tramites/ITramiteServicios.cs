namespace DNP.Backbone.Servicios.Interfaces.Tramites
{
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Dominio.Dto.Proyecto;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;
    using DNP.Backbone.Dominio.Dto.Transversal;

    public interface ITramiteServicios
    {
        Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto);
        Task<InboxTramite> ObtenerInboxTramites(InstanciaTramiteDto instanciaTramiteDto);
        Task<InboxTramite> ObtenerInboxTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto);
        Task<ProyectosTramitesDTO> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto);
        Task<Dominio.Dto.InstanciaResultado> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<TipoTramiteDto>> ObtenerTiposTramites(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP);
        Task<InboxTramite> ObtenerInboxTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP);
        //Task<IEnumerable<ProyectosEnTramiteDto>> ObtenerProyectosPorTramite(int TramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion);
        Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramite(int TipoTramiteId, string Rol,int tramiteId, string usuarioDnpo, string TokenAutorizacion, string nivelId);
        Task<List<FuentePresupuestalDto>> ObtenerFuentesInformacionPresupuestal(string usuarioDnp);

        Task<List<ProyectoFuentePresupuestalDto>> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto, string usuarioDnp);

        Task<List<Dominio.Dto.Tramites.Proyectos.ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, string usuarioDnp, bool isCDP);


        Task<RespuestaGeneralDto> ActualizarInstanciaProyecto(ProyectosTramiteDto parametros, string usuarioDnp);
        Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string IdNivel, string TokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarValoresProyecto(ProyectosTramiteDto parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<List<JustificacionTematicaDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp, string TokenAutorizacion);
        Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId, string usuarioDnpo, string TokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp);

        Task<List<TipoRequisitoDto>> ObtenerTiposRequisito(string usuarioDnp);

        Task<List<FuentesTramiteProyectoAprobacionDto>> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto, string usuarioDnpo, string TokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp);

        Task<CodigoPresupuestalDto> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp);

        Task<RespuestaGeneralDto> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario);
        Task<List<TramitesProyectosDto>> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuario);

        Task<TramiteValoresProyectoDto> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuarioDnp);
        Task<List<ConceptoDireccionTecnicaTramiteDto>> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario);
        Task<RespuestaGeneralDto> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramiteDto> parametros, string usuario);
        RespuestaDocumentoCONPES ObtenerProyectoConpes(string conpes, string idUsuario);
        Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<PlantillaCarta> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite, string usuarioDnp);
        Task<List<Carta>> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId, string usuarioDnp);

        Task<string> ObtenerCartaConceptoDatosDespedida(int tramiteId, int plantillaCartaSeccionId, string IdUsuario, string tokenAutorizacion);
        Task<RespuestaGeneralDto> ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros, string usuarioDnp);

        Task<UsuarioTramite> VerificaUsuarioDestinatario(UsuarioTramite parametros, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarCartaDatosIniciales(Carta parametros, string usuarioDnp);
        Task<List<UsuarioTramite>> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite, string usuarioDnp);
        Task<CapituloConpes> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string idUsuario, string NivelId, string FlujoId);
        Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string idUsuario);
        Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string idUsuario);
        Task<RespuestaGeneralDto> Firmar(int tramiteId, string radicadoSalida, string idUsuario);

        Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes conpes, string idUsuario);
        Task<List<DocumentoCONPES>> EliminarProyectoConpes(string proyectoid, string conpesid, string idUsuario);
        Task<List<CuerpoConceptoCDP>> ObtenerCuerpoConceptoCDP(int tramiteId, string usuarioDnp);
        Task<List<CuerpoConceptoAutorizacion>> ObtenerCuerpoConceptoAutorizacion(int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp);

        Task<Carta> ConsultarCarta(int tramiteId, string usuarioDnp);

        Task<string> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario);
        Task<ResponseDto<bool>> CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDnp);
        Task<ResponseDto<bool>> ConsultarRadicado(string radicado, string usuarioDnp);
        Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto radicado, string usuarioDnp);

        Task<dynamic> CerrarRadicadosTramite(string numeroTramite, string usuarioDnp);

        Task<int> TramiteEnPasoUno(Guid InstanciaId, string usuarioDnp);

        Task<ResponseDto<List<TramiteConpesDetailDto>>> ObtenerTramiteConpes(int tramiteId, string usuarioDnp);

        Task<ResponseDto<bool>> AsociarTramiteConpes(AsociarConpesTramiteRequestDto model, string usuarioDnp);

        Task<ResponseDto<bool>> RemoverAsociacionConpes(RemoverAsociacionConpesTramiteDto model, string usuarioDnp);

        Task<ResponseDto<DetalleTramiteDto>> ObtenerDetalleTramitePorInstancia(string instanciaId, string usuarioDnp);

        Task<ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, string usuarioDnp);

        Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuario);

        Task<List<TramiteProyectoVFODto>> ObtenerProyectoAsociacionVFO(string bpin, int tramite, string tipoTramite, string usuario);

        Task<string> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto, string usuarioDnp);
        Task<DatosProyectoTramiteDto> ObtenerDatosProyectoTramite(int tramiteId, string usuario);

        Task<ResponseDto<bool>> CrearRadicadoEntradaTramite(int tramiteId, string usuarioDnp);

        Task<List<DatosProyectoTramiteDto>> ObtenerDatosProyectosPorTramite(int tramiteId, string usuario);
        #region Vigencias Futuras

        Task<string> ObtenerDatosCronograma(Guid instanciaId, string usuarioDnp);
        Task<List<JustificacionPasoDto>> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp);

        Task<List<TramiteDeflactoresDto>> ObtenerDeflactores(string usuarioDnp);
        Task<List<TramiteProyectoDto>> ObtenerProyectoTramite(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<string> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto, string usuarioDnp);
        Task<VigenciaFuturaCorrienteDto> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin, string usuarioDnp);
        Task<VigenciaFuturaConstanteDto> ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int tramiteId, string usuarioDnp);
        Task<InformacionPresupuestalValoresDto> ObtenerInformacionPresupuestalValores(int tramiteId, string usuario);
        Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuarioDnp);
        Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuarioDnp);
        Task<string> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuarioDnp);
        #endregion Vigencias Futuras

        Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConceptoPorTramite(int tramiteId, string usuarioDnp);
        Task<CrearRadicadoResponseDto> CrearRadicadoSalida(RadicadoSalidaRequestDto parametros, string usuarioDnp);

        Task<int> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, string usuarioDnp, Guid InstanciaId = default(Guid));
        Task<AccionDto> ObtenerAccionActualyFinal(int tramiteId, string bpin, string usuarioDnp);

        Task<List<TramiteModalidadContratacionDto>> ObtenerModalidadesContratacion(int mostrar, string usuarioDnp);
        Task<ActividadPreContractualDto> ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionId, string usuarioDnp);
        Task<ActividadPreContractualDto> ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades, string usuarioDnp);
        Task<RespuestaGeneralDto> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuarioDnp, string usuarioLogueado);
        Task<ProductosConstantesVF> ObtenerProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase, string usuarioDnp);
        Task<ProductosCorrientesVF> ObtenerProductosVigenciaFuturaCorriente(string Bpin, int TramiteId, string usuarioDnp);

        Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string  nivelId, string rolId, string usuarioDnp);
        Task<List<DatosUsuarioDto>> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia, string usuarioDNP);
        Task<ModificacionLeyendaDto> ObtenerModificacionLeyenda(int tramiteId, int ProyectoId, string usuarioDNP);
        Task<string> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuario);
        Task<List<EntidadCatalogoDTDto>> ObtenerListaDirecionesDNP(Guid idEntididad, string usuarioDnp);

        Task<List<EntidadCatalogoDTDto>> ObtenerListaSubdirecionesPorParentId(int idEntididadType, string usuarioDnp);

        Task<RespuestaGeneralDto> BorrarFirma(string idUsuario);
        Task<ProyectosCartaDto> ObtenerProyectosCartaTramite(int TramiteId, string usuarioDnp);
        Task<DetalleCartaConceptoALDto> ObtenerDetalleCartaAL(int TramiteId, string usuarioDnp);
        Task<int> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<DatosProyectoTramiteDto> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId, string usuarioDnp);
        Task<List<TramiteLiberacionVfDto>> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<VigenciaFuturaResponse> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuario);
        Task<VigenciaFuturaResponse> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuario);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp);

        Task<List<EntidadesAsociarComunDto>> obtenerEntidadAsociarProyecto(Guid instanciaId, string AccionTramiteProyecto, string usuarioDnp);
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
        Task<RespuestaGeneralDto> ActualizarCargueMasivo(ObjetoNegocioDto contenido, string usuarioDnp);       
        Task<string> ConsultarCargueExcel(ObjetoNegocioDto contenido, string usuarioDnp);
        Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario);
        Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuario);
        Task<List<EntidadesAsociarComunDto>> ObtenerEntidadTramite(string numeroTramite, string usuarioDnp);
        Task<VigenciaFuturaResponse> EliminarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto eliminarLiberacionVigenciasFuturasDto, string usuarioDnp);
        Task<List<CalendarioPeriodoDto>> ObtenerCalendartioPeriodo(string bpin, string usuarioDnp);
        Task<PresupuestalProyectosAsociadosDto> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId, string usuarioDnp);
        Task<string> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId, string usuarioDnp);
        Task<string> ObtenerResumenReprogramacionPorVigencia(int TramiteId, Guid InstanciaId, int ProyectoId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosReprogramacion(DatosReprogramacionDto Reprogramacion, string usuarioDnp);
        Task<OrigenRecursosDto> GetOrigenRecursosTramite(int TramiteId, string usuarioDnp);
        Task<VigenciaFuturaResponse> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuarioDnp);
        Task<SystemConfigurationDto> ConsultarSystemConfiguracion(string VariableKey, string Separador, string usuarioDnp);
        Task<string> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId, string usuarioDnp);
        Task<int> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp);
        Task<List<TramiteRVFAutorizacionDto>> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramite, string tipoTramite, string usuario);
        Task<string> AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuarioDnp);
        Task<TramiteRVFAutorizacionDto> ObtenerAutorizacionAsociada(int tramiteId, string usuario);
        Task<string> EliminaReprogramacionVF(ReprogramacionDto reprogramacionDto, string usuarioDnp);
    }
}
