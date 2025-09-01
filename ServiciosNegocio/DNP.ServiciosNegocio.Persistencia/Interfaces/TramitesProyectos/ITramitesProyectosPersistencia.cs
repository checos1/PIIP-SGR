using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
namespace DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos
{
    using System;
    using System.Collections.Generic;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;

    public interface ITramitesProyectosPersistencia
    {

        TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario);
        TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario);
        TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);
        IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocio(int TramiteId);
        IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramite(int TipoTramiteId, Guid? Rol, int tramiteId, int nivelId);
        TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId);
        void ActualizarInstanciaProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario);


        IEnumerable<FuentePresupuestalDto> ObtenerFuentesInformacionPresupuestal();
        IEnumerable<ProyectoFuentePresupuestalDto> ObtenerProyectoFuentePresupuestalPorTramite(int pTramiteProyectoId, int? pProyectoFuentePresupuestalId, string pTipoProyecto);
        IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pTramiteProyectoId, int? pProyectoRequisitoId, bool isCDP);
        IEnumerable<JustificacionTramiteProyectoDto> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, Guid IdNivel);
        TramitesResultado GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> justificacionTramiteProyectoDto, string usuario);
        TramitesResultado ActualizarValoresProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario);
        TramitesResultado ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);
        IEnumerable<JustificacionTematicaDto> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId);
        IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId);
        IEnumerable<TipoRequisitoDto> ObtenerTiposRequisito();
        IEnumerable<FuentesTramiteProyectoAprobacionDto> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto);
        TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario);
        CodigoPresupuestalDto ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario);

        TramitesResultado ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario);
        bool CrearAlcanceTramite(AlcanceTramiteDto data);
        ResponseDto<EnvioSubDireccionDto> GuardarSolicitarConcepto(EnvioSubDireccionDto concepto);
        List<EnvioSubDireccionDto> ObtenerSolicitarConcepto(int tramiteid);
        List<TramitesProyectosDto> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuario);
        TramitesValoresProyectoDto ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuario);
        List<ConceptoDireccionTecnicaTramite> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario);
        TramitesResultado GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramite> lConceptoDireccionTecnicaTramite, string usuario);

        TramitesResultado ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);
        PlantillaCarta ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite);
        List<Carta> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId);
        UsuarioTramite VerificaUsuarioDestinatario(UsuarioTramite usuarioTramite);

        TramitesResultado ActualizarCartaDatosIniciales(Carta datosIniciales, string usuario);
        TramitesResultado ActualizarCartaDatosDespedida(Carta datosDespedida, string usuario);
        List<UsuarioTramite> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite);
        TramitesResultado CargarFirma(FileToUploadDto parametros);
        TramitesResultado ValidarSiExisteFirmaUsuario(string idUsuario);
        TramitesResultado Firmar(int tramiteId, string radicadoSalida, string usuario);
        List<Carta> ObtenerDatosCartaPorSeccionDespedia(int plantillaSeccionId, int tramiteId);
        List<CuerpoConceptoCDP> ObtenerDatosCartaConceptoCuerpoCDP(int tramiteId);
        List<CuerpoConceptoAutorizacion> ObtenerDatosCartaConceptoCuerpoAutorizacion(int tramiteId);
        Carta ConsultarCarta(int tramiteid);
        TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario);
        int TramiteEnPasoUno(Guid InstanciaId);

        List<TramiteConpes> ObtenerConpesTramite(int tramiteId);

        void GuardarConpesTramites(int tramiteId, string usuario, List<TramiteConpesDto> jsonConpesModel);

        void RemoverConpesTramites(RemoverAsociacionConpesTramiteDto conpesModel);

        PeriodoPresidencial ObtenerPeriodoPresidencialActual();

        string EliminarAsociacionVFO(EliminacionAsociacionDto tramiteFiltroDto);

        List<proyectoAsociarTramite> ObtenerProyectoAsociarTramite(string bpin, int tramiteId);

        string AsociarProyectoVFO(proyectoAsociarTramite proyectoDto, string usuario);

        DatosProyectoTramiteDto ObtenerDatosProyectoTramite(int tramiteId);

        CartaConcepto ObtenerDetalleCartaConcepto(int tramiteId);
        List<DatosProyectoTramiteDto> ObtenerDatosProyectosPorTramite(int tramiteId);

        #region Vigencias Futuras

        InformacionPresupuestalVlrConstanteDto ObtenerInformacionPresupuestalVlrConstanteVF(int tramiteId);

        string ObtenerDatosCronograma(Guid instanciaId);

        IEnumerable<JustificacionPasoDto> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId);

        InformacionPresupuestalValoresDto ObtenerInformacionPresupuestalValores(int tramiteId);
        string GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuario);

        #endregion Vigencias Futuras

        List<EnvioSubDireccionDto> ObtenerSolicitarConceptoPorTramite(int tramiteId);
        List<TramiteDeflactoresDto> GetTramiteDeflactores();
        List<Dominio.Dto.Tramites.TramiteProyectoDto> GetProyectoTramite(int ProyectoId, int TramiteId);
        string ActualizaVigenciaFuturaProyectoTramite(Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto, string usuario);
        VigenciaFuturaCorrienteDto GetFuentesFinanciacionVigenciaFuturaCorriente(string Bpin);
        VigenciaFuturaConstanteDto GetFuentesFinanciacionVigenciaFuturaCoonstante(string Bpin, int TramiteId);
        AccionDto ObtenerAccionActualyFinal(int tramiteId, string bpin);
        int EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid));
        VigenciaFuturaResponse ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuario);
        VigenciaFuturaResponse ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuario);
        TramitesResultado EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuario);
        List<TramiteModalidadContratacionDto> ObtenerModalidadesContratacion(int? mostrar);
        ActividadPreContractualDto ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionId, string usuario);
        ActividadPreContractualDto ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades);
        ProductosConstantesVF GetProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase);
        ProductosCorrientesVF GetProductosVigenciaFuturaCorriente(string Bpin, int TramiteId);

        IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId);
        IEnumerable<DatosUsuarioDto> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia);
        List<proyectoAsociarTramite> ObtenerProyectoAsociarTramiteLeyenda(string bpin, int tramiteId);
        ModificacionLeyendaDto ObtenerModificacionLeyenda(int tramiteId, int ProyectoId);
        string ActualizaModificacionLeyenda(Dominio.Dto.Tramites.ModificacionLeyendaDto modificacionLeyendaDto, string usuario);
        List<EntidadCatalogoDTDto> ObtenerListaDireccionesDNP(Guid idEntididad);
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
        int ValidacionPeriodoPresidencial(int TramiteId);
        TramitesResultado GuardarMontosTramite(List<ProyectosEnTramiteDto> parametrosGuardar, string usuario);
        List<tramiteVFAsociarproyecto> ObtenerTramitesVFparaLiberar(int proyectoId);
        string GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuario); 
        IEnumerable<ProyectoJustificacioneDto> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel);
        List<ResumenLiberacionVfDto> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId);
        ValoresUtilizadosLiberacionVfDto ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId);
        int TramiteAjusteEnPasoUno(int tramiteId, int proyectoId);
        List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId);
        VigenciaFuturaResponse InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario);
        VigenciaFuturaResponse InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosCorrientes, string usuario);
        List<EntidadesAsociarComunDto> ObtenerEntidadTramite(string numeroTramite);
        VigenciaFuturaResponse EliminaLiberacionVF(LiberacionVigenciasFuturasDto tramiteEliminar);
        List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId);
        List<CalendarioPeriodoDto> ObtenerCalendartioPeriodo(string bpin);
        PresupuestalProyectosAsociadosDto ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId);
        string ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId);
        OrigenRecursosDto GetOrigenRecursosTramite(int TramiteId);
        VigenciaFuturaResponse SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuario);
        int ObtenerModalidadContratacionVigenciasFuturas( int ProyectoId, int TramiteId);
        List<TramiteRVFAutorizacion> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteId);
        string AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuario);
        TramiteRVFAutorizacion ObtenerAutorizacionAsociada(int tramiteId);
        VigenciaFuturaResponse EliminaReprogramacionVF(ReprogramacionDto tramiteEliminar);

    }
}
