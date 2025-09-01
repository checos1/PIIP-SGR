namespace DNP.Backbone.Web.API.Test.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Comunes.Dto;
    using DNP.Backbone.Dominio;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Flujos;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Programacion; 
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.Flujos.Dominio.Dto.Flujos;
    using Servicios.Interfaces.ServiciosNegocio;

    public class FlujoServiciosMock : IFlujoServicios
    {
        public Task<Comunes.Dto.InstanciaDto> ActivarInstancia(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<AlertasConfigDto> ActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<AlertasConfigDto> CrearAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<Comunes.Dto.InstanciaDto> DetenerInstancia(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<Comunes.Dto.InstanciaDto> CancelarInstanciaMisProcesos(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<Dominio.Dto.InstanciaResultado> EliminarInstanciasPermiso(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<Dominio.Dto.InstanciaResultado> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dominio.Dto.InstanciaResultado>> GenerarInstancias(Dominio.Dto.ParametrosInstanciaDto parametros)
        {
            return Task.FromResult(new List<Dominio.Dto.InstanciaResultado>() { new Dominio.Dto.InstanciaResultado() });
        }

        public Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto instanciaAlertasConfigDto)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto alertasGeneradasFiltro)
        {
            throw new NotImplementedException();
        }

        public Task<IList<FlujosLogsInstanciasDto>> ObtenerFlujoLogInstancia(Guid flujoId, string usuarioDnp)
        {
            IList<FlujosLogsInstanciasDto> res = new List<FlujosLogsInstanciasDto>
            {
                new FlujosLogsInstanciasDto()
            };
            return Task.FromResult(res);
        }

        public Task<FlujoMenuContextualDto> ObtenerFlujoPorInstanciaTarea(string usuarioDnp, Guid idInstancia)
        {
            if((usuarioDnp == "jdelgado")&&(idInstancia == Guid.Parse("24054986-A739-4990-A1C7-B662274873DF")))
            {
                return Task.FromResult(new FlujoMenuContextualDto()
                {
                    Id = Guid.Parse("4e957ac3-64a5-4e7f-8377-27cec03a1aab"),
                    Nombre = "Flujo Complejo PBI 2320",
                    Descripcion = "Flujo Complejo PBI 2320",
                    IdInstancia = Guid.Parse("24054986-a739-4990-a1c7-b662274873df")
                });
            }

            return null;
        }

        public Task<List<InfoFinancieroProyectoDto>> ObtenerInfoFinancieroProyectos(InfoFinancieroProyectoFiltroDto infoFinancieroProyectoFiltro)
        {
            throw new NotImplementedException();
        }

        public Task<Comunes.Dto.InstanciaDto> ObtenerInstanciaPorId(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FlujosProgramacionDto>> ObtenerListaFlujosTramitePorNivel(Guid idNivel, string idUsuarioDNP)
        {
            IEnumerable<FlujosProgramacionDto> result = new List<FlujosProgramacionDto>() { new FlujosProgramacionDto() };
            return Task.FromResult(result);
        }

        public Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivas(ParametrosObjetosNegocioDto parametros)
        {
            return Task.FromResult(new List<NegocioDto>());
        }

        public Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadas(ParametrosObjetosNegocioDto parametros)
        {
            return Task.FromResult(new List<NegocioDto>());
        }

        public Task<IList<LogsInstanciasDto>> ObtenerLogInstancia(ParametrosLogsInstanciasDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<IList<SubpasoDto>> ObtenerLogSubpasos(Guid idInstancia, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        
        public Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasTotales(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<List<MapColumnasDto>> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<NegocioDto>> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<NegocioDto>> ObtenerProyectosTramiteConsola(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<int, bool>> ObtenerSituacaoAlertasProyectos(InstanciaProyectoDto proyectoDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<TramiteDto>> ObtenerTramites(TramiteFiltroDto parametros)
        {
            return Task.FromResult(new List<TramiteDto>());
        }

        public Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new NotImplementedException();
        }

        public Task<Comunes.Dto.InstanciaDto> PausarInstancia(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<Dominio.Dto.InstanciaResultado> RegistrarPermisosInstancias(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultadoValidarProyectoItemDto>> ValidarProyectosConInstanciasActivas(ValidarProyectosDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<List<NegocioDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadasConsolaProcesos(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }

        public Task<List<TramiteDto>> ObtenerTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<AutorizacionAccionesPorInstanciaDto>> ObtenerInstanciasPermiso(ParametrosObjetosNegocioDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ObtenerInstanciasActivasProyectos(List<string> Bpins, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<DNP.Backbone.Dominio.Dto.InstanciaResultado>> GenerarInstanciasMasivo(List<ParametrosInstanciaDto> parametros)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ObtenerInstanciasActivasProyectos(string Bpins, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<long>> ObtenerTarmitesEstadoCerrado(int proyectoId, int entidadId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> ObtenerTramitesInstanciasEstadoCerrado(int proyectoId, int entidadId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DNP.Backbone.Dominio.Dto.InstanciaResultado> EliminarInstanciaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<TrazaAccionesPorInstanciaDto> ObtenerObservacionesPasoPadre(Guid idInstancia, Guid idAccion, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DNP.Backbone.Dominio.Dto.InstanciaResultado> CrearLogFlujo(FlujosLogsInstanciasDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<IList<FlujosLogsInstanciasDto>> ObtenerFlujoLogInstancia(Guid instanciaId, Guid nivelId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<HistoricoObservacionesDto>> ObtenerHistoricoObservaciones(Guid instanciaId, string usuarioDnp)
        {
            return Task.FromResult(new List<HistoricoObservacionesDto>());
        }

        public AccionesPorInstanciaDto ConsultarAccionPorInstancia(Guid idInstancia, Guid idAccion, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        public Task<Dominio.Dto.InstanciaResultado> RegistrarPermisosAccionPorUsuario(RegistrarPermisosAccionDto permisosAccion)
        {
            throw new NotImplementedException();
        }

        public Task<DNP.Backbone.Dominio.Dto.InstanciaResultado> CerrarInstancia(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DetalleTramiteDto> ObtenerDetallesTramite(string numerotramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comunes.Dto.InstanciaDto>> DevolverInstanciasHijas(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }
        public Task<ProyectoTramiteDto> ObtenerProyectosPorTramite(Guid? instanciaId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DetalleTramiteDto> ObtenerDetallesTramitePorInstancia(string instanciaId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, int anoInicial, int anoFinal, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, DateTime fechaiInicial, DateTime fechaFinal, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaParametrosValidarFlujoDto> ValidarFlujoConInstanciaActiva(ParametrosValidarFlujoDto parametros, string usuarioDnp)
        {
            RespuestaParametrosValidarFlujoDto data = new RespuestaParametrosValidarFlujoDto();
            return Task.FromResult(data);
        }
        public Task<List<LogDto>> ObtenerLog(Guid instanciaId, string usuarioDnp)
        {
            List<LogDto> data = new List<LogDto>();
            data.Add(new LogDto() { IdInstanciaEnvio= Guid.Parse("BE35A03F-1D13-476C-B14E-96524FACBE1F"), ProcesoEnvio= "Tramite de traslado de ley", PasoEnvio= "Asociar Proyectos", CodigoEnvio = "EJ-TP-TL-0-0002", UsuarioEnvio= "CC202002", FechaEnvio=DateTime.Now, RolEnvio= "R_Presupuesto - preliminar", EntidadEnvio=182, NombreEntidadEnvio= "MINISTERIO DE DEFENSA NACIONAL - FUERZA AEREA" });
            var result = data.Where(x => x.IdInstanciaEnvio == instanciaId);
            return Task.FromResult(result.ToList());
        }

       

        public Task<List<Dominio.Dto.Flujos.InstanciaResultado>> CrearInstancia(ParametrosInstanciaFlujoDto parametrosInstanciaDto, string usuarioDnp)
        {
            parametrosInstanciaDto.UsuarioId = "CC202002";
            parametrosInstanciaDto.RolId = new Guid("DA595AA3-CF59-46D3-A22A-0D96DA5C7371");
            parametrosInstanciaDto.ObjetoId = "EJ-TP-TO-120101-0150";
            parametrosInstanciaDto.TipoObjetoId = new Guid("9C5EF8C1-DA05-48B9-BA29-00C9EFD7A774");
            parametrosInstanciaDto.FlujoId = new Guid("23EE582A-08C2-43F5-A60C-425608FF9D81");
            parametrosInstanciaDto.Descripcion = "Prueba Definitiva del Trámite de Traslado Ordinario";

            Dominio.Dto.Flujos.InstanciaResultado Resultado = new Dominio.Dto.Flujos.InstanciaResultado
            {
                Exitoso = true,
                MensajeOperacion = "OK"
            };
            List<Dominio.Dto.Flujos.InstanciaResultado> lResultado = new List<Dominio.Dto.Flujos.InstanciaResultado>();
            lResultado.Add(Resultado);

            return Task.FromResult(lResultado.ToList());
        }

        public Task<List<OpcionFlujoDto>> ObtenerPermisosFlujosPorAplicacionYRoles(FiltroConsultaOpcionesDto filtroConsulta, string usuarioDnp)
        {
            filtroConsulta.IdAplicacion ="App:ipp";
            OpcionFlujoDto objeto = new OpcionFlujoDto
            {
                Activo=true,
                 NombreOpcion="Ejemplo",
                 IdNivel= new Guid("DA595AA3-CF59-46D3-A22A-0D96DA5C7371")
            };

            List<OpcionFlujoDto> lobjeto = new List<OpcionFlujoDto>();
            lobjeto.Add(objeto);
            return Task.FromResult(lobjeto.ToList());
        }

        public Task<List<ProyectoEntidadDto>> ConsultarProyectosEntidadesSinInstanciasActivas(ParametrosProyectosFlujosDto parametros, string usuarioDnp)
        {

            parametros.IdUsuarioDNP = "CC202002";

            ProyectoEntidadDto entidad = new ProyectoEntidadDto
            {
                CodigoBpin = "202200000000150",
                EntidadNombre = "Ministerio",
                ProyectoId = 98765
            };
            List<ProyectoEntidadDto> lentidad = new List<ProyectoEntidadDto>();
            lentidad.Add(entidad);

            return Task.FromResult(lentidad.ToList());
        }

		public Task<bool> ObtenerValidacionVerAccion(ValidarRolAccionDto parametros, string usuarioDnp)
		{
			throw new NotImplementedException();
		}

		public Task<InstanciaProyectoDto> ObtenerInstanciaProyecto(Guid idInstancia, string bpin, string idusuarioDnp)
		{
			throw new NotImplementedException();
		}

		public Task<int> CrearTrazaAccionesPorInstancia(TrazaAccionesPorInstanciaDto parametros, string idusuarioDnp)
		{
			throw new NotImplementedException();
		}

		public Task<List<DevolucionAccionesDto>> ObtenerDevolucionesPorIdInstanciaYIdAccion(Guid idInstancia, Guid bpin, string idusuarioDnp)
		{
			throw new NotImplementedException();
		}

		public Task<ResultadoEjecucionFlujoDto> EjecutarFlujo(ParametrosEjecucionFlujo parametrosEjecucionFlujo, string idusuarioDnp)
		{
			throw new NotImplementedException();
		}

		public Task<ResultadoDevolverFlujoDto> DevolverFlujo(ParametrosDevolverFlujoDto parametrosDevolucionFlujo, string usuarioDnp)
		{
			throw new NotImplementedException();
		}

        public Task<DNP.Backbone.Dominio.Dto.InstanciaResultado> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp)
        {
            DNP.Backbone.Dominio.Dto.InstanciaResultado rta = new DNP.Backbone.Dominio.Dto.InstanciaResultado();
            if (instanciaTramite == new Guid("00000000-0000-0000-0000-000000000000") || instanciaTramite == null)
                rta.Exitoso = false;
            else
                rta.Exitoso = true;
            return Task.FromResult(rta); 
        }

        public Task<DNP.Backbone.Dominio.Dto.InstanciaResultado> NotificarUsuariosPorInstanciaPadre(Guid instanciaId, string nombreNotificacion, string texto, string usuarioDnp)
        {
            DNP.Backbone.Dominio.Dto.InstanciaResultado rta = new DNP.Backbone.Dominio.Dto.InstanciaResultado();
            if (instanciaId == new Guid("00000000-0000-0000-0000-000000000000") || instanciaId == null)
                rta.Exitoso = false;
            else
                rta.Exitoso = true;
            return Task.FromResult(rta);
        }

        public Task<List<TipoTramiteDto>> ObtenerTiposTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            List<TipoTramiteDto> tipos = new List<TipoTramiteDto>();
            tipos.Add(new TipoTramiteDto() { Id = 1, Nombre = "Vigencias" });
            tipos.Add(new TipoTramiteDto() { Id = 1, Nombre = "Tramites" });
            return Task.FromResult(tipos);
        }

        public Task<List<FlujoDto>> ObtenerFlujosPorTipoObjeto(Guid tipoObjetoId, string usuarioDnp)
        {
            List<FlujoDto> flujos = new List<FlujoDto>();
            flujos.Add(new FlujoDto() { Id = Guid.NewGuid(), Nombre = "Flujo Prueba" });
            return Task.FromResult(flujos);
        }

        public Task<List<AccionesFlujosDto>> ObtenerAccionesFlujoPorFlujoId(Guid flujoId, string usuarioDnp)
        {
            List<AccionesFlujosDto> flujos = new List<AccionesFlujosDto>();
            flujos.Add(new AccionesFlujosDto() { Id = Guid.NewGuid(), Nombre = "Accion Prueba" });
            return Task.FromResult(flujos);
        }

        public Task<List<int>> ObtenerVigencias(Guid tipoObjetoId, string usuarioDnp)
        {
            List<int> años = new List<int>();
            años.Add(2023);
            return Task.FromResult(años);
        }

        public Task<List<TramiteDto>> ObtenerTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new List<TramiteDto>());
        }

        public Task<bool> ExisteFlujoProgramacion(int entidadId, Guid flujoId, string usuarioDnp)
        {
            return Task.FromResult(true);
        }

        public Task<List<TramiteDto>> ObtenerProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new List<TramiteDto>());
        }

        public Task<string> ObtenerEstadoOcultarObservacionesGenerales(string usuarioDnp)
        {
            var data = string.Empty;
            return Task.FromResult(data);
        }

        public Task<bool> SubPasoEjecutar(ParametrosEjecucionSubPasoDto oParametrosEjecucionSubPasoDto, string usuarioDnp)
        {
            return Task.FromResult(true);
        }

        public Task<EstadoFlujoResultado> SubPasosValidar(Guid idInstancia, Guid idAccion, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<NegocioVerificacionOcadPazDto>> ObtenerListaObjetosNegocioConInstanciasActivasYPausadasVerificacionOcadPazSgr(ParametrosObjetosNegocioDto parametros)
        {
            throw new NotImplementedException();
        }


        public Task<List<TrazaAccionDto>> ObtenerTrazaInstancia(Guid idInstancia, string usuario)
        {
            throw new NotImplementedException();

        }
    }
}
