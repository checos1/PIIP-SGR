using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
namespace DNP.ServiciosNegocio.Servicios.Interfaces.TramitesProyectos
{
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;

    public interface ITramitesProyectosServicio
    {
        ParametrosGuardarDto<DatosTramiteProyectosDto> ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto contenido);
        TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);

        TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario);

        TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario);
        IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocio(int TramiteId);

        IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramite(int TipoTramiteId, Guid? Rol, int tramiteId, int nivelId);
        IEnumerable<FuentePresupuestalDto> ObtenerFuentesInformacionPresupuestal();
        IEnumerable<Dominio.Dto.Tramites.ProyectoFuentePresupuestalDto> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto);
        IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP);
        TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId);
        void ActualizarInstanciaProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario);
        IEnumerable<JustificacionTramiteProyectoDto> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, Guid IdNivel);
        TramitesResultado GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> justificacionTramiteProyectoDto, string usuario);
        TramitesResultado ActualizarValoresProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario);
        TramitesResultado ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);
        IEnumerable<JustificacionTematicaDto> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId);

        IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId);

        IEnumerable<TipoRequisitoDto> ObtenerTiposRequisito();



        IEnumerable<FuentesTramiteProyectoAprobacionDto> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyectod);
        TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario);

        CodigoPresupuestalDto ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario);
        TramitesResultado ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario);
        bool CrearAlcanceTramite(AlcanceTramiteDto data);
        ResponseDto<EnvioSubDireccionDto> GuardarSolicitarConcepto(EnvioSubDireccionDto concepto);
        List<EnvioSubDireccionDto> ObtenerSolicitarConcepto(int tramiteid);
        List<TramitesProyectosDto> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuarioDnp);
        TramitesValoresProyectoDto ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuario);
        List<ConceptoDireccionTecnicaTramite> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string name);
        TramitesResultado GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramite> lConceptoDireccionTecnicaTramite, string name);
        TramitesResultado ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);

        PlantillaCarta ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite);
        List<Carta> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId);
        List<Carta> ObtenerDatosCartaPorSeccionDespedia(int plantillaSeccionId, int tramiteId);
        ////Alejandro
        //List<Carta> ObtenerDatosCartaConceptoDespedida(int tramiteId);
        UsuarioTramite VerificaUsuarioDestinatario(UsuarioTramite usuarioTramite);

        TramitesResultado ActualizarCartaDatosIniciales(Carta datosIniciales, string usuario);
        TramitesResultado ActualizarCartaDatosDespedida(Carta datosDespedida, string usuario);
        List<UsuarioTramite> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite);
        TramitesResultado CargarFirma(FileToUploadDto parametros);
        TramitesResultado ValidarSiExisteFirmaUsuario(string idUsuario);
        TramitesResultado Firmar(int tramiteId, string radicadoSalida, string usuario);
        List<CuerpoConceptoCDP> ObtenerDatosCartaConceptoCuerpoCDP(int tramiteId);
        List<CuerpoConceptoAutorizacion> ObtenerDatosCartaConceptoCuerpoAutorizacion(int tramiteId);
        Carta ConsultarCarta(int tramiteid);
        TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario);
        int TramiteEnPasoUno(Guid InstanciaId);

        ResponseDto<List<TramiteConpesDto>> ObtenerConpesTramite(int tramiteId);

        ResponseDto<bool> GuardarConpesTramite(AsociarTramiteConpesRequestDto model, string usuario);

        ResponseDto<bool> RemoverConpesTramite(RemoverAsociacionConpesTramiteDto model);

        ResponseDto<PeriodoPresidencialDto> ObtenerPeriodoPresidencialActual();

        string EliminarAsociacionVFO(EliminacionAsociacionDto tramiteFiltroDto);
        List<proyectoAsociarTramite> ObtenerProyectoAsociarTramite(string bpin, int tramiteId);
        string AsociarProyectoVFO(proyectoAsociarTramite proyectoDto, string usuario);

        DatosProyectoTramiteDto ObtenerDatosProyectoTramite(int tramiteId);

        DetalleCartaConceptoDto ObtenerDetalleCartaConcepto(int tramiteId);
        List<DatosProyectoTramiteDto> ObtenerDatosProyectosPorTramite(int tramiteId);
        int TramiteAjusteEnPasoUno(int tramiteId, int proyectoId);
        #region Vigencias Futuras

        InformacionPresupuestalVlrConstanteDto ObtenerInformacionPresupuestalVlrConstanteVF(int tramiteId);

        string ObtenerDatosCronograma(Guid instanciaId);
        IEnumerable<JustificacionPasoDto> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId);

        List<TramiteDeflactoresDto> GetTramiteDeflactores();
        List<Dominio.Dto.Tramites.TramiteProyectoDto> GetProyectoTramite(int ProyectoId, int TramiteId);  
        string ActualizaVigenciaFuturaProyectoTramite(Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto, string usuario);
        InformacionPresupuestalValoresDto ObtenerInformacionPresupuestalValores(int tramiteId);
        VigenciaFuturaResponse ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuario);
        VigenciaFuturaResponse ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuario);
        string GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuario);

        #endregion Vigencias Futuras

        List<EnvioSubDireccionDto> ObtenerSolicitarConceptoPorTramite(int tramiteId);
        VigenciaFuturaCorrienteDto GetFuentesFinanciacionVigenciaFuturaCorriente(string Bpin);
        VigenciaFuturaConstanteDto GetFuentesFinanciacionVigenciaFuturaCoonstante(string Bpin, int TramiteId);

        AccionDto ObtenerAccionActualyFinal(int tramiteId, string bpin);

        int EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid));

        TramitesResultado EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuario);

        List<TramiteModalidadContratacionDto> ObtenerModalidadesContratacion(int? mostrar);
        ActividadPreContractualDto ActualizarActividadesCronograma(ActividadPreContractualDto actividades, string usuario);
        ActividadPreContractualDto ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades);
        ProductosConstantesVF GetProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase);
        ProductosCorrientesVF GetProductosVigenciaFuturaCorriente(string Bpin, int TramiteId);

        IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId);
        IEnumerable<DatosUsuarioDto> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia);
        List<proyectoAsociarTramite> ObtenerProyectoAsociarTramiteLeyenda(string bpin, int tramiteId);
        ModificacionLeyendaDto ObtenerModificacionLeyenda(int tramiteId, int ProyectoId);
        string ActualizaModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuario);

        List<EntidadCatalogoDTDto> ObtenerListaDireccionesDNP(Guid IdEntidad);
        List<EntidadCatalogoDTDto> ObtenerListaSubdireccionesPorParentId(int idEntididadType);
        TramitesResultado BorrarFirma(FileToUploadDto parametros);
        ProyectosCartaDto ObtenerProyectosCartaTramite(int tramiteId);
        DetalleCartaConceptoALDto ObtenerDetalleCartaAL(int tramiteId);
        int ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId);
        DatosProyectoTramiteDto ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId);
        List<TramiteLiberacionVfDto> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId);
        VigenciaFuturaResponse InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuario);
        VigenciaFuturaResponse InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuario);
        List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId);

        List<EntidadesAsociarComunDto> ObtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto);
        CartaConcepto ConsultarCartaConcepto(int tramiteid);
        int ValidacionPeriodoPresidencial(int tramiteid);
        TramitesResultado GuardarMontosTramite(List<ProyectosEnTramiteDto> parametrosGuardar, string usuario);
        List<tramiteVFAsociarproyecto> ObtenerTramitesVFparaLiberar(int proyectoId);
        string GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuario);
        IEnumerable<ProyectoJustificacioneDto> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel);
        List<ResumenLiberacionVfDto> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId);
        ValoresUtilizadosLiberacionVfDto ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId);
        List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId);
        VigenciaFuturaResponse InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario);
        VigenciaFuturaResponse InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuario);
        List<EntidadesAsociarComunDto> ObtenerEntidadTramite(string numeroTramite);
        VigenciaFuturaResponse EliminaLiberacionVF(LiberacionVigenciasFuturasDto tramiteEliminar);
        List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId);
        List<CalendarioPeriodoDto> ObtenerCalendartioPeriodo(string bpin);
        PresupuestalProyectosAsociadosDto ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId);
        string ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId);
        OrigenRecursosDto GetOrigenRecursosTramite(int TramiteId);
        VigenciaFuturaResponse SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuario);
        int ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId);
        List<TramiteRVFAutorizacion> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteId);
        string AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuario);
        TramiteRVFAutorizacion ObtenerAutorizacionAsociada(int tramiteId);
        VigenciaFuturaResponse EliminaReprogramacionVF(ReprogramacionDto tramiteEliminar);

    }
}
