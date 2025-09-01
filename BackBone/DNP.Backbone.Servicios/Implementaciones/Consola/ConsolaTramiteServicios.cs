namespace DNP.Backbone.Servicios.Implementaciones.Consola
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Consola;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using Interfaces.ServiciosNegocio;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase responsable de la gestión de servicio del trámites
    /// </summary>
    public class ConsolaTramiteServicios : IConsolaTramiteServicios
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionServicios;
        private IClienteHttpServicios _clienteHttpServicios;
        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>                
        public ConsolaTramiteServicios(IFlujoServicios flujoServicios, IAutorizacionServicios autorizacionServicios, IClienteHttpServicios clienteHttpServicios)
        {
            _flujoServicios = flujoServicios;
            _autorizacionServicios = autorizacionServicios;
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos trámite.</returns>
        public async Task<ConsolaTramiteDto> ObtenerConsolaTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            var tramites = await _flujoServicios.ObtenerTramites(instanciaTramiteDto);
            var inbox = new ConsolaTramiteDto();
            if (tramites != null && tramites.Any())
            {
                inbox.ListaGrupoTramiteEntidad = await CrearGrupoTramitePorEntidad(tramites);
            }
            else
            {
                inbox.ListaGrupoTramiteEntidad = new List<GrupoTramiteEntidad>();
            }
            return inbox;
        }

        /// <summary>
        /// Obtención lista de grupo de tramites.
        /// </summary>
        /// <param name="tramites">lista de trámites</param>
        /// <returns>consulta de datos de lista de grupo de trámite.</returns>
        public async Task<List<GrupoTramiteEntidad>> CrearGrupoTramitePorEntidad(List<TramiteDto> tramites)
        {
            var grupoTramite = from t in tramites
                               group t by new { t.TipoTramiteId, t.NombreTipoTramite, t.TipoEntidadId, t.NombreEntidad, t.SectorId, t.NombreSector, t.NombreTipoEntidad, t.EntidadId }
                               into e
                               select new GrupoTramites()
                               {
                                   TipoTramiteId = e.Key.TipoTramiteId,
                                   NombreTipoTramite = e.Key.NombreTipoTramite,
                                   NombreSector = e.Key.NombreSector,
                                   SectorId = e.Key.SectorId,
                                   TipoEntidadId = e.Key.TipoEntidadId,
                                   NombreTipoEntidad = e.Key.NombreTipoEntidad,
                                   EntidadId = e.Key.EntidadId,
                                   NombreEntidad = e.Key.NombreEntidad,
                                   ListaTramites = e.ToList()
                               };


            var groupEntidad = from g in grupoTramite
                               group g by new { g.EntidadId, g.NombreEntidad, g.SectorId, g.NombreSector }
                               into e
                               select new GrupoTramiteEntidad()
                               {
                                   IdSector = e.Key.SectorId.Value,
                                   Sector = e.Key.NombreSector,
                                   EntidadId = e.Key.EntidadId,
                                   NombreEntidad = e.Key.NombreEntidad,
                                   GrupoTramites = e.ToList()
                               };

            return await Task.Run(() => groupEntidad.ToList());
        }

        /// <summary>
        /// Obtención lista de proyectos por trámite.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos de lista de proyectos por trámite.</returns>
        public async Task<ProyectosTramitesDTO> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            var proyectosTramites = new ProyectosTramitesDTO();
            var proyectos = _flujoServicios.ObtenerProyectosTramite(instanciaTramiteDto).Result;
            if (proyectos != null && proyectos.Any())
            {
                proyectosTramites.ListaProyectos = proyectos;
            }
            else
            {
                proyectosTramites.ListaProyectos = new List<NegocioDto>();
                var instancia = _flujoServicios.ObtenerInstanciaPorId(instanciaTramiteDto);
                proyectosTramites.NombreTramite = instancia.Result?.Descripcion;
            }

            return await Task.Run(() => proyectosTramites);
        }



        /// <summary>
        /// Obtención lista de archivos.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos de lista de proyectos por trámite.</returns>
        public async Task<List<DocumentoDto>> ObtenerDocumentos(string idInstancia, string idUsuarioDNP)
        {
            //var urlBase = "https://as-manejadorarchivos-api-ntuat.azurewebsites.net/";
            var urlBase = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
            var uri = "ArchivoInfo/f76bca19-8116-4157-aa89-d8a41759e79d";
            
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, new { idinstancia = idInstancia }, idUsuarioDNP, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<DocumentoDto>>(respuesta);
        }

        public async Task<Guid?> ObtenerIdAplicacionPorInstancia(Guid idInstancia, string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerIdAplicacionPorInstancia"];
            var parametros = $"?idInstancia={idInstancia}";

            var idAplicacion = JsonConvert.DeserializeObject<Guid?>(
                await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp));

            return idAplicacion;
        }

        public async Task<List<MacroProcesosCantidadDto>> ObtenerMacroProcesosCantidad(string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var AplicacionBackboneId = ConfigurationManager.AppSettings["AplicacionBackboneId"];
            var uriMetodo = "api/Flujos/ObtenerMacroProcesosCantidad";
            var parametros = $"?AplicacionBackboneId={AplicacionBackboneId}&usuario={usuarioDnp}";

            var data = JsonConvert.DeserializeObject<List<MacroProcesosCantidadDto>>(
                await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp));

            return data;
        }

        public async Task<List<MacroProcesosDto>> ObtenerMacroProcesos(string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var AplicacionBackboneId = ConfigurationManager.AppSettings["AplicacionBackboneId"];
            var uriMetodo = "api/Flujos/ObtenerMacroProcesos";
            var parametros = $"?AplicacionBackboneId={AplicacionBackboneId}";

            var data = JsonConvert.DeserializeObject<List<MacroProcesosDto>>(
                await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp));

            return data;
        }

        public async Task<List<MacroProcesosDto>> ObtenerProcesos(string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var AplicacionBackboneId = ConfigurationManager.AppSettings["AplicacionBackboneId"];
            var uriMetodo = "api/Flujos/ObtenerProcesos";
            var parametros = $"?AplicacionBackboneId={AplicacionBackboneId}";

            var data = JsonConvert.DeserializeObject<List<MacroProcesosDto>>(
                await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp));

            return data;
        }
    }
}
