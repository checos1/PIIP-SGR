namespace DNP.Backbone.Servicios.Interfaces.ServiciosNegocio
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Flujos;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Programacion;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.Flujos.Dominio.Dto.Flujos;
	using System.Web.Http;

	public interface IFlujoServicios
    {
        Task<FlujoMenuContextualDto> ObtenerFlujoPorInstanciaTarea(string usuarioDnp, Guid idInstancia);
        Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivas(ParametrosObjetosNegocioDto parametros);
        Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasTotales(ParametrosObjetosNegocioDto parametros);
        Task<List<ResultadoValidarProyectoItemDto>> ValidarProyectosConInstanciasActivas(ValidarProyectosDto dto);
        Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<TramiteDto>> ObtenerTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<NegocioDto>> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto instanciaAlertasConfigDto);
        Task<List<MapColumnasDto>> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto);
        Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto);
        Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto);
        Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto alertasGeneradasFiltro);
        Task<List<InfoFinancieroProyectoDto>> ObtenerInfoFinancieroProyectos(InfoFinancieroProyectoFiltroDto infoFinancieroProyectoFiltro);
        Task<IDictionary<int, bool>> ObtenerSituacaoAlertasProyectos(InstanciaProyectoDto proyectoDto);
        Task<List<NegocioDto>> ObtenerProyectosTramiteConsola(InstanciaTramiteDto instanciaTramiteDto);
        Task<InstanciaDto> ObtenerInstanciaPorId(InstanciaTramiteDto instanciaTramiteDto);
        Task<IEnumerable<FlujosProgramacionDto>> ObtenerListaFlujosTramitePorNivel(Guid idNivel, string idUsuarioDNP);
        Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadas(ParametrosObjetosNegocioDto parametros);
        Task<InstanciaDto> ActivarInstancia(ParametrosObjetosNegocioDto parametros);
        Task<InstanciaDto> PausarInstancia(ParametrosObjetosNegocioDto parametros);
        Task<InstanciaDto> DetenerInstancia(ParametrosObjetosNegocioDto parametros);
        Task<InstanciaDto> CancelarInstanciaMisProcesos(ParametrosObjetosNegocioDto parametros);
        Task<List<Dominio.Dto.InstanciaResultado>> GenerarInstancias(Dominio.Dto.ParametrosInstanciaDto parametros);
        Task<IList<LogsInstanciasDto>> ObtenerLogInstancia(ParametrosLogsInstanciasDto parametros, string usuarioDnp);
        
        Task<Dominio.Dto.InstanciaResultado> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto);
        Task<Dominio.Dto.InstanciaResultado> RegistrarPermisosInstancias(ParametrosObjetosNegocioDto parametros);
        Task<Dominio.Dto.InstanciaResultado> EliminarInstanciasPermiso(ParametrosObjetosNegocioDto parametros);
        Task<Dominio.Dto.InstanciaResultado> CrearLogFlujo(FlujosLogsInstanciasDto parametros, string usuarioDnp);
        Task<IList<FlujosLogsInstanciasDto>> ObtenerFlujoLogInstancia(Guid instanciaId, Guid nivelId, string usuarioDnp);
        Task<List<HistoricoObservacionesDto>> ObtenerHistoricoObservaciones(Guid instanciaId, string usuarioDnp);

        Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadasConsolaProcesos(ParametrosObjetosNegocioDto parametros);
        Task<List<TipoTramiteDto>> ObtenerTiposTramites(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<TramiteDto>> ObtenerTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<TramiteDto>> ObtenerProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<AutorizacionAccionesPorInstanciaDto>> ObtenerInstanciasPermiso(ParametrosObjetosNegocioDto parametros, string usuarioDnp);
        Task<RespuestaParametrosValidarFlujoDto> ValidarFlujoConInstanciaActiva(ParametrosValidarFlujoDto parametros, string usuarioDnp);
        Task<List<string>> ObtenerInstanciasActivasProyectos(string Bpins, string usuarioDnp);
        Task<List<Dominio.Dto.InstanciaResultado>> GenerarInstanciasMasivo(List<Dominio.Dto.ParametrosInstanciaDto> parametros);
        Task<List<int>> ObtenerTramitesInstanciasEstadoCerrado(int proyectoId, int entidadId, string usuarioDnp);

        Task<Dominio.Dto.InstanciaResultado> EliminarInstanciaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp);
        Task<TrazaAccionesPorInstanciaDto> ObtenerObservacionesPasoPadre(Guid idInstancia, Guid idAccion, string usuarioDnp);
        Task<Dominio.Dto.InstanciaResultado> RegistrarPermisosAccionPorUsuario(Dominio.RegistrarPermisosAccionDto permisosAccion);
        AccionesPorInstanciaDto ConsultarAccionPorInstancia(Guid idInstancia, Guid idAccion, string usuarioDnp);
        Task<Dominio.Dto.InstanciaResultado> CerrarInstancia(int tramiteId, string usuarioDnp);
        Task<DetalleTramiteDto> ObtenerDetallesTramite(string numerotramite, string usuarioDnp);

        Task<List<InstanciaDto>> DevolverInstanciasHijas(ParametrosObjetosNegocioDto parametros);
        Task<ProyectoTramiteDto> ObtenerProyectosPorTramite(Guid? instanciaId, string usuarioDnp);
        Task<DetalleTramiteDto> ObtenerDetallesTramitePorInstancia(string instanciaId, string usuarioDnp);

        Task<Dominio.Dto.ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, DateTime fechaiInicial, DateTime fechaFinal, string usuarioDnp);

        Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuarioDnp);
        Task<List<LogDto>> ObtenerLog(Guid instanciaId, string usuarioDnp);

        Task<IList<SubpasoDto>> ObtenerLogSubpasos(Guid idInstancia, string usuarioDnp);

        Task<List<TrazaAccionDto>> ObtenerTrazaInstancia(Guid idInstancia, string usuarioDnp);
        Task<List<Dominio.Dto.Flujos.InstanciaResultado>> CrearInstancia(ParametrosInstanciaFlujoDto parametrosInstanciaDto, string usuarioDnp);
        Task<List<OpcionFlujoDto>> ObtenerPermisosFlujosPorAplicacionYRoles(FiltroConsultaOpcionesDto filtroConsulta, string usuarioDnp);
        Task<string> ObtenerEstadoOcultarObservacionesGenerales(string usuarioDnp);
        Task<List<ProyectoEntidadDto>> ConsultarProyectosEntidadesSinInstanciasActivas(ParametrosProyectosFlujosDto parametros, string usuarioDnp);

        Task<bool> ObtenerValidacionVerAccion(ValidarRolAccionDto parametros, string usuarioDnp);
        Task<InstanciaProyectoDto> ObtenerInstanciaProyecto(Guid idInstancia, string bpin, string usuarioDnp);
        Task<int> CrearTrazaAccionesPorInstancia(TrazaAccionesPorInstanciaDto parametros, string usuarioDnp);
        Task<List<DevolucionAccionesDto>> ObtenerDevolucionesPorIdInstanciaYIdAccion(Guid idInstancia, Guid bpin, string usuarioDnp);
        Task<ResultadoEjecucionFlujoDto> EjecutarFlujo(ParametrosEjecucionFlujo parametrosEjecucionFlujo, string usuarioDnp);
        Task<ResultadoDevolverFlujoDto> DevolverFlujo(ParametrosDevolverFlujoDto parametrosDevolucionFlujo, string usuarioDnp);
        Task<Dominio.Dto.InstanciaResultado> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp);
        Task<Dominio.Dto.InstanciaResultado> NotificarUsuariosPorInstanciaPadre(Guid instanciaId, string nombreNotificacion, string texto, string usuarioDnp);
        Task<List<FlujoDto>> ObtenerFlujosPorTipoObjeto(Guid tipoObjetoId, string usuarioDnp);
        Task<List<AccionesFlujosDto>> ObtenerAccionesFlujoPorFlujoId(Guid flujoId, string usuarioDnp);
        Task<List<int>> ObtenerVigencias(Guid tipoObjetoId, string usuarioDnp);
        Task<bool> ExisteFlujoProgramacion(int entidadId, Guid flujoId, string usuarioDnp);
        Task<EstadoFlujoResultado> SubPasosValidar(Guid idInstancia, Guid idAccion, string usuarioDnp);
        Task<bool> SubPasoEjecutar(ParametrosEjecucionSubPasoDto oParametrosEjecucionSubPasoDto, string usuarioDnp);
        Task<List<NegocioVerificacionOcadPazDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadasVerificacionOcadPazSgr(ParametrosObjetosNegocioDto parametros);
    }
}
