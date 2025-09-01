namespace DNP.Backbone.Servicios.Interfaces.SGR
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.DesignacionEjecutor;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.SGR;
    using DNP.Backbone.Dominio.Dto.SGR.AvalUso;
    using DNP.Backbone.Dominio.Dto.SGR.CTEI;
    using DNP.Backbone.Dominio.Dto.SGR.CTUS;
    using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
    using DNP.Backbone.Dominio.Dto.SGR.OcadPaz;
    using DNP.Backbone.Dominio.Dto.SGR.Reportes;
    using DNP.Backbone.Dominio.Dto.SGR.Transversal;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISGRServicios
    {
        /// <summary>
        /// llamado al servicio para consultar operaciones de credito
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="instanciaId"></param>
        /// <returns></returns>
        Task<OperacionCreditoDatosGeneralesDto> ObtenerOperacionCreditoDatosGenerales(string bpin, string usuarioDnp);

        /// <summary>
        /// llamado al servicio para guardar operaciones de credito
        /// </summary>
        /// <param name="OperacionCreditoDatosGeneralesDto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        Task<string> GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuarioDnp);

        /// <summary>
        /// llamado al servicio para eliminar datos generales y detealle de operación de crédito
        /// </summary>
        /// <param name="EliminarOperacionCreditoSGR"></param>
        /// <param name="proyectoid"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        Task<string> EliminarOperacionCreditoSGR(int proyectoid, string usuarioDnp);

        /// <summary>
        /// llamado al servicio para consultar detalles de operaciones de credito
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="instanciaId"></param>
        /// <returns></returns>
        Task<OperacionCreditoDetallesDto> ObtenerOperacionCreditoDetalles(string bpin, string usuarioDnp);

        /// <summary>
        /// llamado al servicio para guardar detalles deoperaciones de credito
        /// </summary>
        /// <param name="OperacionCreditoDetallesDto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        Task<string> GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuarioDnp);

        /// <summary>
        /// Obtiene los ejecutores de acuerdo al tipo de Entidad
        /// </summary>
        /// <param name="idTipoEntidad"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns></returns>
        Task<List<EjecutorCatalogoDto>> ObtenerEjecutorByTipoEntidad(int idTipoEntidad, string usuarioDNP);

        /// <summary>
        /// Obtiene los ejecutores de acuerdo a los filtros indicados
        /// </summary>
        /// <param name="nit"></param>
        /// <param name="tipoEntidadId"></param>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        Task<List<EjecutorEntidadDto>> ObtenerListadoEjecutores(string nit, int? tipoEntidadId, int? entidadId, string usuarioDNP);

        /// <summary>
        /// Obtener Listado de Ejecutores Asociados
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <returns></returns>
        Task<List<EjecutorEntidadAsociado>> ObtenerListadoEjecutoresAsociados(int proyectoId, string usuarioDNP);

        /// <summary>
        /// Permite guardar el ejecutor asociado
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <param name="ejecutorId"></param>
        /// <param name="usuario"></param>
        /// <param name="tipoEjecutorId"></param>
        /// <returns></returns>
        Task<bool> CrearEjecutorAsociado(int proyectoId, int ejecutorId, string usuario, int tipoEjecutorId);

        /// <summary>
        /// Permite eliminar a un Ejecutor Asociado
        /// </summary>
        /// <param name="EjecutorAsociadoId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        Task<SeccionesEjecutorEntidad> EliminarEjecutorAsociado(int EjecutorAsociadoId, string usuario);

        Task<DesagregarRegionalizacionDto> ObtenerDesagregarRegionalizacionSgr(string bpin, string usuario);
        //Task<List<DatosGeneralesProyectosDto>> ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId, string usuario);
        Task<FocalizacionPoliticaSgrDto> ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin, string usuario);
        Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario);
        //Task<List<PoliticasTCrucePoliticasDto>> ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente, string usuario);
        //Task<List<string>> ObtenerDatosIndicadoresPoliticaSgr(string bpin, string usuario);
        Task<string> ObtenerCategoriaProductosPoliticaSgr(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion);

        Task<string> ObtenerPoliticasTransversalesProyectoSgr(string bpin, string usuarioDnp);
        Task<string> EliminarPoliticasProyectoSgr(int proyectoId, int politicaId, string usuarioDnp);
        Task<string> AgregarPoliticasTransversalesAjustesSgr(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP);
        Task<string> ConsultarPoliticasCategoriasIndicadoresSgr(System.Guid instanciaId, string usuarioDnp);
        Task<string> ObtenerPoliticasTransversalesCategoriasSgr(System.Guid instanciaId, string usuarioDnp);
        Task<string> EliminarCategoriasPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId, string usuarioDnp);
        Task<string> ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar, string usuarioDnp);
        Task<string> ObtenerCrucePoliticasAjustesSgr(System.Guid instanciaId, string usuarioDnp);
        Task<string> GuardarCrucePoliticasAjustesSgr(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP);
        Task<string> ObtenerPoliticasTransversalesResumenSgr(System.Guid instanciaId, string usuarioDnp);
        Task<List<TipoDocumentoSoporteDto>> ObtenerTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, string usuarioDnp, string nivelId, string instanciaId, string accionId);
        Task<List<TipoDocumentoSoporteDto>> ObtenerListaTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, string usuarioDnp, string nivelId, string instanciaId, string accionId);

        Task<List<ProyectoViabilidadInvolucradosDto>> LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId, string usuarioDnp);
        Task<List<ProyectoViabilidadInvolucradosFirmaDto>> LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp);
        Task<string> GuardarProyectoViabilidadInvolucrados(ProyectoViabilidadInvolucradosDto objProyectoViabilidadInvolucradosDto, string usuarioDNP);
        Task<string> EliminarProyectoViabilidadInvolucradoso(int id, string usuarioDnp);
        Task<string> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir, string usuarioDnp);
        Task<ProyectoCtusDto> SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId, string usuarioDnp);
        Task<List<EntidadesSolicitarCtusDto>> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId, string usuarioDnp);
        Task<string> GuardarProyectoSolicitarCTUS(ProyectoCtusDto objProyectoCtusDto, string usuarioDNP);
        Task<ConfiguracionReportesDto> SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId, string usuarioDNP);
        Task<bool> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId, string usuarioDNP);

        Task<ValidacionOCADPazDto> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId, string usuarioDNP);

        #region Ocad Paz
        Task<List<UsuariosVerificacionOcadPazDto>> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(Guid rolId, int entidadId, string usuarioDNP);
        Task<ResultadoProcedimientoDto> SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto obj, string usuarioDNP);
        #endregion
        Task<List<MensajeDto>> SGR_Proyectos_validarTecnicoOcadpaz(Guid instanciaId, Guid accionId, string usuarioDNP);
        Task<string> ValidarInstanciaCTUSNoFinalizada(int idProyecto, string usuarioDNP);

        Task<int> ValidarViavilidadCumplimentoFlujoSGR(Guid instanciaId, string usuarioDnp);

        Task<bool> TieneInstanciaActiva(String ObjetoNegocioId, string usuarioDnp);

        #region Firma viabilidad
        Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string usuarioDnp);
        Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string usuarioDnp);
        Task<RespuestaGeneralDto> Firmar(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId);
        Task<RespuestaGeneralDto> EliminarFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId);
        Task<RespuestaGeneralDto> BorrarFirma(string usuarioDnp);
        #endregion

        #region Entidad Nacional SGR

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>List<EntidadesAdscritasDto></returns> 
        Task<List<EntidadesAdscritasDto>> SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad, string usuarioDnp);

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipo"></param>  
        /// <param name="usuarioDnp"></param>  
        /// <returns>Json</returns> 
        Task<ResultadoProcedimientoDto> SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo, string usuarioDnp);

        /// <summary>
        /// Validar usuario encargado
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="instanciaId"></param>  
        /// <param name="usuarioDnp"></param>  
        /// <returns>UsuarioEncargadoDto</returns> 
        Task<List<ListaUsuarioDto>> SGR_Proyectos_LeerAsignacionUsuarioEncargado(int proyectoId, Guid instanciaId, string usuarioDnp);

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="delegado"></param> 
        /// <param name="usuarioDnp"></param> 
        /// <returns>int</returns> 
        Task<bool> SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, int entityId, bool delegado, string usuarioDnp);

        /// <summary>
        /// Guardar asignacion usuario encargado
        /// </summary> 
        /// <param name="json"></param> 
        /// <param name="usuarioDnp"></param> 
        /// <returns>ResultadoProcedimientoDto</returns> 
        Task<ResultadoProcedimientoDto> SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json, string usuarioDnp);

        #endregion Entidad Nacional SGR

        #region CTEI

        Task<string> SGR_Proyectos_LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId, string usuarioDnp);

        Task<string> SGR_Proyectos_GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto obj, string usuarioDNP);

        #endregion CTEI

        #region Aval de Uso
        Task<string> SGR_Proyectos_RegistrarAvalUsoSgr(DatosAvalUsoDto obj, string usuarioDNP);

        Task<string> SGR_Proyectos_LeerAvalUsoSgr(int proyectoId, Guid instanciaId, string usuarioDnp);
        #endregion

        #region Priorizacion
        Task<List<EstadosPriorizacionDto>> SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId, string usuarioDNP);
        #endregion

        #region Aprobación
        Task<IEnumerable<ProyectoAprobacionInstanciasDto>> ObtenerProyectoAprobacionInstanciasSGR(Nullable<Guid> instanciaId, string usuarioDNP);
        Task<ProyectoAprobacionInstanciasResultado> GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto, string UsuarioDNP);
        Task<ProyectoProcesoResultado> GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto, string UsuarioDNP);
        Task<IEnumerable<ProyectoResumenEstadoAprobacionCreditoDto>> ObtenerProyectoResumenEstadoAprobacionCreditoSGR(string proyectoId, string usuarioDNP);

        Task<IEnumerable<ProyectoAprobacionResumenDto>> ObtenerProyectoResumenAprobacionSGR(string proyectoId, string usuarioDNP);
        Task<IEnumerable<ProyectoAprobacionResumenDto>> ObtenerProyectoResumenAprobacionCreditoParcialSGR(string proyectoId, string usuarioDNP);
        #endregion

        #region Designacion Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <param name="usuarioDNP"></param>
        /// <returns>bool</returns> 
        Task<bool> RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores, string usuarioDNP);

        /// <summary>
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns>string</returns>
        Task<string> ObtenerRespuestaEjecutorSGR(string campo, int proyectoId, string usuarioDNP);

        /// <summary>
        /// Obtiene los valores de la aprobación por proyectoId.
        /// </summary>    
        /// <param name="proyectoId"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns>string</returns>
        Task<string> LeerValoresAprobacionSGR(int proyectoId, string usuarioDNP);

        /// <summary>
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>  
        /// <param name="valores"></param>
        /// <param name="usuarioDNP"></param>
        /// <returns>bool</returns>
        Task<bool> ActualizarValorEjecutorSGR(CampoItemValorDto valores, string usuarioDNP);

        /// <summary>
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>  
        /// <param name="instanciaId"></param>     
        /// <returns>bool</returns>
        Task<string> ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId, string usuarioDNP);

        #endregion Designacion Ejecutor

        Task<List<EjecutorEntidadAsociado>> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecutorId, string usuarioDNP);

    }
}
