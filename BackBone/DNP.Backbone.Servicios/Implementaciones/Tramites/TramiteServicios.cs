namespace DNP.Backbone.Servicios.Implementaciones.Tramites
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Dominio.Dto.Usuario;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Interfaces.ServiciosNegocio;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;
    using DNP.Backbone.Dominio.Dto.Transversal;

    /// <summary>
    /// Clase responsable de la gestión de servicio del trámites
    /// </summary>
    public class TramiteServicios : ITramiteServicios
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionServicios;
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>                
        public TramiteServicios(IFlujoServicios flujoServicios, IAutorizacionServicios autorizacionServicios, IServiciosNegocioServicios serviciosNegocioServicios, IClienteHttpServicios clienteHttpServicios)
        {
            _flujoServicios = flujoServicios;
            _autorizacionServicios = autorizacionServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            this._clienteHttpServicios = clienteHttpServicios;

        }

        /// <summary>
        /// Obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos trámite.</returns>
        public Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return _flujoServicios.ObtenerTramites(instanciaTramiteDto);
        }

        /// <summary>
        /// Obtención de datos de grupo de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos de grupo de trámite.</returns>
        public async Task<InboxTramite> ObtenerInboxTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            var tipoEntidad = instanciaTramiteDto.TramiteFiltroDto?.FiltroGradeDtos?.FirstOrDefault(x => x.Campo.Equals("NombreTipoEntidad"));
            var tramites = await _flujoServicios.ObtenerTramites(instanciaTramiteDto);
            var inbox = new InboxTramite();
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
        /// Obtención de datos de grupo de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos de grupo de trámite.</returns>
        public async Task<InboxTramite> ObtenerInboxTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto)
        {
            var tipoEntidad = instanciaTramiteDto.TramiteFiltroDto?.FiltroGradeDtos?.FirstOrDefault(x => x.Campo.Equals("NombreTipoEntidad"));
            var tramites = await _flujoServicios.ObtenerTramitesProgramacion(instanciaTramiteDto);
            var inbox = new InboxTramite();
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
                               group t by new { t.NombreTipoTramite, t.NombreEntidad, t.SectorId, t.NombreSector, t.NombreTipoEntidad, t.EntidadId, t.NombreFlujo }
                               into e
                               select new GrupoTramites()
                               {
                                   NombreTipoTramite = e.Key.NombreTipoTramite,
                                   NombreSector = e.Key.NombreSector,
                                   SectorId = e.Key.SectorId,
                                   NombreTipoEntidad = e.Key.NombreTipoEntidad,
                                   EntidadId = e.Key.EntidadId,
                                   NombreEntidad = e.Key.NombreEntidad,
                                   NombreFlujo = e.Key.NombreFlujo,
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

        public Task<Dominio.Dto.InstanciaResultado> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return _flujoServicios.EliminarProyectoTramite(instanciaTramiteDto);
        }

        /// <summary>
        /// Obtención de datos de grupo de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos de grupo de trámite.</returns>
        public async Task<InboxTramite> ObtenerInboxTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
            var tipoEntidad = instanciaTramiteDto.TramiteFiltroDto?.FiltroGradeDtos?.FirstOrDefault(x => x.Campo.Equals("NombreTipoEntidad"));

            if (!string.IsNullOrEmpty(usuarioDNP))
            {
                var entidadesVisualizador = await _autorizacionServicios.ObtenerEntidadesConRoleVisualizador(usuarioDNP);
                var entidadesVisualizadorIds = entidadesVisualizador.Where(x => x.EntityTypeCatalogOptionId.HasValue).Select(x => x.EntityTypeCatalogOptionId.Value);

                instanciaTramiteDto.TramiteFiltroDto.EntidadesVisualizador = entidadesVisualizadorIds.ToList();
            }

            var tramites = await _flujoServicios.ObtenerTramitesConsolaProcesos(instanciaTramiteDto);
            var inbox = new InboxTramite();
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

        public async Task<List<TipoTramiteDto>> ObtenerTiposTramites(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
           var tipoEntidad = instanciaTramiteDto.TramiteFiltroDto?.FiltroGradeDtos?.FirstOrDefault(x => x.Campo.Equals("NombreTipoEntidad"));

            if (!string.IsNullOrEmpty(usuarioDNP))
            {
                var entidadesVisualizador = await _autorizacionServicios.ObtenerEntidadesConRoleVisualizador(usuarioDNP);
                var entidadesVisualizadorIds = entidadesVisualizador.Where(x => x.EntityTypeCatalogOptionId.HasValue).Select(x => x.EntityTypeCatalogOptionId.Value);

                instanciaTramiteDto.TramiteFiltroDto.EntidadesVisualizador = entidadesVisualizadorIds.ToList();
            }

            var tiposTramites = await _flujoServicios.ObtenerTiposTramites(instanciaTramiteDto);
            return tiposTramites;
        }



        public async Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnp, string TokenAutorizacion)
        {
            var proyectosTramites = new List<ProyectosEnTramiteDto>();
            proyectosTramites = _serviciosNegocioServicios.ObtenerProyectosTramiteNegocio(TramiteId, usuarioDnp, TokenAutorizacion).Result;
            return await Task.Run(() => proyectosTramites);
        }

        public async Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramite(int TipoTramiteId, string Rol, int tramiteId, string usuarioDnp, string TokenAutorizacion, string nivelId)
        {
            var tipoDocumentoTramite = new List<TipoDocumentoTramiteDto>();
            tipoDocumentoTramite = _serviciosNegocioServicios.ObtenerTipoDocumentoTramite(TipoTramiteId, Rol, tramiteId, usuarioDnp, TokenAutorizacion, nivelId).Result;
            return await Task.Run(() => tipoDocumentoTramite);
        }

        public async Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarProyectosTramiteNegocio(parametros, usuarioDnp);
        }

        public Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            return _serviciosNegocioServicios.EliminarProyectoTramiteNegocio(instanciaTramiteDto);
        }

        public async Task<RespuestaGeneralDto> ActualizarInstanciaProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarInstanciaProyecto(parametros, usuarioDnp);
        }

        public async Task<List<ProyectoFuentePresupuestalDto>> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto, string usuarioDNP)
        {
            return await _serviciosNegocioServicios.ObtenerProyectoFuentePresupuestalPorTramite(pProyectoId, pTramiteId, pTipoProyecto, usuarioDNP);

        }

        public async Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, string usuarioDNP, bool isCDP)
        {
            return await _serviciosNegocioServicios.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, usuarioDNP, isCDP);
        }

        public async Task<List<Dominio.Dto.FuentePresupuestalDto>> ObtenerFuentesInformacionPresupuestal(string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerFuentesInformacionPresupuestal(usuarioDnp);

        }

        public async Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string IdNivel, string TokenAutorizacion)
        {
            var justificacionTramiteProyecto = new List<JustificacionTramiteProyectoDto>();
            justificacionTramiteProyecto = _serviciosNegocioServicios.ObtenerPreguntasJustificacion(TramiteId, ProyectoId, TipoTramiteId, TipoRolId, usuarioDnp, IdNivel, TokenAutorizacion).Result;
            return await Task.Run(() => justificacionTramiteProyecto);

        }
        public async Task<RespuestaGeneralDto> ActualizarValoresProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarValoresProyecto(parametros, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarTramiteInformacionPresupuestal(parametros, usuarioDnp);
        }
        public async Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarTramiteTipoRequisito(parametros, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarRespuestasJustificacion(parametros, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ValidarEnviarDatosTramiteNegocio(parametros, usuarioDnp);
        }

        public async Task<List<JustificacionTematicaDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp, string TokenAutorizacion)
        {
            var preguntasProyectoActualizacion = new List<JustificacionTematicaDto>();
            preguntasProyectoActualizacion = _serviciosNegocioServicios.ObtenerPreguntasProyectoActualizacion(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId, usuarioDnp, TokenAutorizacion).Result;
            return await Task.Run(() => preguntasProyectoActualizacion);

        }

        public async Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId, string usuarioDnp, string TokenAutorizacion)
        {
            var proyectosTramites = new List<ProyectosEnTramiteDto>();
            proyectosTramites = _serviciosNegocioServicios.ObtenerProyectosTramiteNegocioAprobacion(TramiteId, TipoRolId, usuarioDnp, TokenAutorizacion).Result;
            return await Task.Run(() => proyectosTramites);
        }

        public async Task<List<TipoRequisitoDto>> ObtenerTiposRequisito(string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerTiposRequisito(usuarioDnp);
        }

        public async Task<List<FuentesTramiteProyectoAprobacionDto>> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto, string usuarioDnp, string TokenAutorizacion)
        {
            var proyectosTramites = new List<FuentesTramiteProyectoAprobacionDto>();
            proyectosTramites = _serviciosNegocioServicios.ObtenerFuentesTramiteProyectoAprobacion(tramiteId, proyectoId, pTipoProyecto, usuarioDnp, TokenAutorizacion).Result;
            return await Task.Run(() => proyectosTramites);
        }

        public async Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarFuentesTramiteProyectoAprobacion(parametros, usuarioDnp);
        }

        public async Task<CodigoPresupuestalDto> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerCodigoPresupuestal(proyectoId, entidadId, tramiteId, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarCodigoPresupuestal(proyectoId, entidadId, tramiteId, usuarioDnp);
        }

        public async Task<List<TramitesProyectosDto>> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerTarmitesPorProyectoEntidad(proyectoId, entidadId, usuarioDnp);
        }

        public async Task<TramiteValoresProyectoDto> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerValoresProyectos(proyectoId, tramiteId, entidadId, usuarioDnp);
        }

        public async Task<List<ConceptoDireccionTecnicaTramiteDto>> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario)
        {
            return await _serviciosNegocioServicios.ObtenerConceptoDireccionTecnicaTramite(tramiteId, nivelid, usuario);
        }

        public async Task<RespuestaGeneralDto> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramiteDto> parametros, string usuario)
        {
            return await _serviciosNegocioServicios.GuardarConceptoDireccionTecnicaTramite(parametros, usuario);
        }

        public RespuestaDocumentoCONPES ObtenerProyectoConpes(string conpes, string idUsuario)
        {
            return _serviciosNegocioServicios.ObtenerProyectoConpes(conpes, idUsuario);
        }

        public async Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ValidarEnviarDatosTramiteNegocioAprobacion(parametros, usuarioDnp);
        }

        public async Task<PlantillaCarta> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerPlantillaCarta(nombreSeccion, tipoTramite, usuarioDnp);
        }
        public async Task<List<Carta>> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDatosCartaPorSeccion(tramiteId, plantillaSeccionId, usuarioDnp);
        }
        //Alejandro
        //public async Task<List<Carta>> ObtenerDatosCartaConceptoDespedida(int tramiteId, string usuarioDnp)
        //{
        //    return await _serviciosNegocioServicios.ObtenerDatosCartaConceptoDespedida(tramiteId, usuarioDnp);
        //}

        public async Task<string> ObtenerCartaConceptoDatosDespedida(int tramiteId, int plantillaCartaSeccionId, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarCartaConceptoDatosDespedida"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?tramiteId={tramiteId}&plantillaCartaSeccionId={plantillaCartaSeccionId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<RespuestaGeneralDto> ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarCartaConceptoDatosDespedida"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }
        public async Task<UsuarioTramite> VerificaUsuarioDestinatario(UsuarioTramite parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.VerificaUsuarioDestinatario(parametros, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> ActualizarCartaDatosIniciales(Carta parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarCartaDatosIniciales(parametros, usuarioDnp);
        }

        public async Task<List<UsuarioTramite>> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerUsuariosRegistrados(tramiteId, numeroTramite, usuarioDnp);
        }

        public async Task<CapituloConpes> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string idUsuario, string NivelId, string FlujoId)
        {
            return await _serviciosNegocioServicios.CargarProyectoConpes(proyectoid, InstanciaId, GuiMacroproceso, idUsuario, NivelId, FlujoId);
        }

        public async Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string idUsuario)
        {
            return await _serviciosNegocioServicios.CargarFirma(firma, rolId, idUsuario);
        }

        public async Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string idUsuario)
        {
            return await _serviciosNegocioServicios.ValidarSiExisteFirmaUsuario(idUsuario);
        }

        public async Task<RespuestaGeneralDto> Firmar(int tramiteId, string radicadoSalida, string idUsuario)
        {
            return await _serviciosNegocioServicios.Firmar(tramiteId, radicadoSalida, idUsuario);
        }

        public Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes conpes, string idUsuario)
        {
            return _serviciosNegocioServicios.AdicionarProyectoConpes(conpes, idUsuario);
        }

        public async Task<List<DocumentoCONPES>> EliminarProyectoConpes(string proyectoid, string conpesid, string idUsuario)
        {
            return await _serviciosNegocioServicios.EliminarProyectoConpes(proyectoid, conpesid, idUsuario);
        }

        public async Task<List<CuerpoConceptoCDP>> ObtenerCuerpoConceptoCDP(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerCuerpoConceptoCDP(tramiteId, usuarioDnp);
        }

        public async Task<List<CuerpoConceptoAutorizacion>> ObtenerCuerpoConceptoAutorizacion(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerCuerpoConceptoAutorizacion(tramiteId, usuarioDnp);
        }

        public async Task<Carta> ConsultarCarta(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ConsultarCarta(tramiteId, usuarioDnp);
        }

        public async Task<string> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario)
        {
            return await _serviciosNegocioServicios.ReasignarRadicadoORFEO(parametros, usuario);
        }

        public async Task<ResponseDto<bool>> CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.CargarDocumentoElectronicoOrfeo(datosDocumentoElectronicoDSDto, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion, usuarioDnp);
        }

        public async Task<ResponseDto<bool>> ConsultarRadicado(string radicado, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ConsultarRadicado(radicado, usuarioDnp);
        }

        public async Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto radicado, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.CerrarRadicado(radicado, usuarioDnp);
        }

        public async Task<dynamic> CerrarRadicadosTramite(string numeroTramite, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.CerrarRadicadosTramite(numeroTramite, usuarioDnp);
        }

        public async Task<int> TramiteEnPasoUno(Guid InstanciaId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.TramiteEnPasoUno(InstanciaId, usuarioDnp);
        }

        public async Task<ResponseDto<List<TramiteConpesDetailDto>>> ObtenerTramiteConpes(int tramiteId, string usuarioDnp)
        {
            var responseDto = new ResponseDto<List<TramiteConpesDetailDto>>();

            try
            {
                responseDto = await _serviciosNegocioServicios.ObtenerConpesTramite(tramiteId, usuarioDnp);
                if (responseDto == null || !responseDto.Estado)
                {
                    throw new Exception(
                        responseDto == null ? "No fue posible consultar los CONPES asociados" : responseDto.Mensaje
                    );
                }
            }
            catch (Exception ex)
            {
                responseDto.Mensaje = ex.Message;
            }

            return responseDto;
        }

        public async Task<ResponseDto<bool>> AsociarTramiteConpes(AsociarConpesTramiteRequestDto model, string usuarioDnp)
        {
            var responseDto = new ResponseDto<bool>();

            try
            {
                responseDto = await _serviciosNegocioServicios.AsociarConpesTramite(model, usuarioDnp);
                if (responseDto == null || !responseDto.Estado)
                {
                    throw new Exception(
                        responseDto == null ? "No fue posible asociar los CONPES seleccionados" : responseDto.Mensaje
                    );
                }
            }
            catch (Exception ex)
            {
                responseDto.Mensaje = ex.Message;
            }

            return responseDto;
        }

        public async Task<ResponseDto<bool>> RemoverAsociacionConpes(RemoverAsociacionConpesTramiteDto model, string usuarioDnp)
        {
            var responseDto = new ResponseDto<bool>();

            try
            {
                responseDto = await _serviciosNegocioServicios.RemoverAsociacionConpesTramite(model, usuarioDnp);
                if (responseDto == null || !responseDto.Estado)
                {
                    throw new Exception(
                        responseDto == null ? "No fue posible remover la asociación del CONPES seleccionado" : responseDto.Mensaje
                    );
                }
            }
            catch (Exception ex)
            {
                responseDto.Mensaje = ex.Message;
            }

            return responseDto;
        }

        public async Task<ResponseDto<DetalleTramiteDto>> ObtenerDetalleTramitePorInstancia(string instanciaId, string usuarioDnp)
        {
            var responseDto = new ResponseDto<DetalleTramiteDto>();

            try
            {
                var query = await _flujoServicios.ObtenerDetallesTramitePorInstancia(instanciaId, usuarioDnp);
                if (query == null)
                {
                    responseDto.Mensaje = responseDto == null ? "No fue posible obtener el detalle del trámite" : responseDto.Mensaje;
                }
                else
                {
                    responseDto.Estado = true;
                    responseDto.Data = query;
                }
            }
            catch (Exception ex)
            {
                responseDto.Mensaje = ex.Message;
            }

            return responseDto;
        }

        public async Task<ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, string usuarioDnp)
        {
            var responseDto = new ResponseDto<bool>();

            try
            {
                var periodoPresidencial = await _serviciosNegocioServicios.ObtenerPeriodoPresidencial(usuarioDnp);
                if (!periodoPresidencial.Estado)
                {
                    responseDto.Mensaje = periodoPresidencial.Mensaje;
                    responseDto.Estado = false;
                }
                else
                {
                    if (periodoPresidencial.Data == null)
                    {
                        responseDto.Data = false;
                        responseDto.Estado = true;

                        return responseDto;
                    }

                    responseDto = await _flujoServicios.ValidarConpesTramiteVigenciaFutura(
                        tramiteId,
                        periodoPresidencial.Data.FechaInicial,
                        periodoPresidencial.Data.FechaFinal,
                        usuarioDnp
                    );
                    if (responseDto.Estado)
                    {
                        responseDto.Estado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                responseDto.Mensaje = ex.Message;
            }

            return responseDto;
        }

        public async Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuario)
        {

            string result;
            result = await _serviciosNegocioServicios.EliminarAsociacionVFO(eliminacionAsociacionDto, usuario);
            if (result == "Desasociación Exitosa")
            {
                result = await _flujoServicios.EliminarAsociacionVFO(eliminacionAsociacionDto, usuario);
                return result;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<TramiteProyectoVFODto>> ObtenerProyectoAsociacionVFO(string bpin, int tramite, string tipoTramite, string usuario)
        {
            return await _serviciosNegocioServicios.ObtenerProyectoAsociacionVFO(bpin, tramite, tipoTramite, usuario);
        }

        public async Task<string> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.AsociarProyectoVFO(tramiteProyectoVFODto,usuarioDnp);
        }

        public async Task<DatosProyectoTramiteDto> ObtenerDatosProyectoTramite(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDatosProyectoTramite(tramiteId, usuarioDnp);
        }

        public async Task<ResponseDto<bool>> CrearRadicadoEntradaTramite(int tramiteId, string usuarioDnp)
        {
            var responseDto = new ResponseDto<bool>();

            try {
                var request = await _serviciosNegocioServicios.CrearRadicadoEntradaTramite(tramiteId, usuarioDnp);
                responseDto.Estado = true;
                responseDto.Data = true;
            } catch(Exception e) {
                responseDto.Mensaje = e.Message;
            }

            return responseDto;
        }

        public async Task<List<DatosProyectoTramiteDto>> ObtenerDatosProyectosPorTramite(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDatosProyectosPorTramite(tramiteId, usuarioDnp);
        }
        #region Vigencias Futuras

        public async Task<string> ObtenerDatosCronograma(Guid instanciaId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDatosCronograma(instanciaId, usuarioDnp);
        }

        public async Task<List<JustificacionPasoDto>> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp)
        {
            var preguntasProyectoActualizacionPaso = new List<JustificacionPasoDto>();
            preguntasProyectoActualizacionPaso = _serviciosNegocioServicios.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId, usuarioDnp).Result;
            return await Task.Run(() => preguntasProyectoActualizacionPaso);

        }

        public async Task<List<TramiteDeflactoresDto>> ObtenerDeflactores(string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDeflactores(usuarioDnp);
        }

        public async Task<List<TramiteProyectoDto>> ObtenerProyectoTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerProyectoTramite(ProyectoId, TramiteId, usuarioDnp);
        }

        public async Task<string> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto, usuarioDnp);
        }

        public async Task<VigenciaFuturaCorrienteDto> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerFuentesFinanciacionVigenciaFuturaCorriente(bpin, usuarioDnp);
        }

        public async Task<VigenciaFuturaConstanteDto> ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, tramiteId, usuarioDnp);
        }

        public async Task<InformacionPresupuestalValoresDto> ObtenerInformacionPresupuestalValores(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerInformacionPresupuestalValores(tramiteId, usuarioDnp);
        }

        public async Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarVigenciaFuturaFuente(fuente, usuarioDnp);
        }

        public async Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarVigenciaFuturaProducto(prod, usuarioDnp);
        }

        public async Task<string> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarInformacionPresupuestalValores(informacionPresupuestalValoresDto, usuarioDnp);
        }
        #endregion Vigencias Futuras    

        public async Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConceptoPorTramite(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerSolicitarConceptoPorTramite(tramiteId, usuarioDnp);
        }

        public async Task<CrearRadicadoResponseDto> CrearRadicadoSalida(RadicadoSalidaRequestDto radicado, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.CrearRadicadoSalida(radicado, usuarioDnp);
        }

        public async Task<int> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, string usuarioDnp, Guid InstanciaId = default(Guid))
        {
            return await _serviciosNegocioServicios.EliminarPermisosAccionesUsuarios(usuarioDestino, tramiteId, aliasNivel, usuarioDnp,InstanciaId);
        }

        public async Task<AccionDto> ObtenerAccionActualyFinal(int tramiteId, string bpin, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerAccionActualyFinal( tramiteId,  bpin,  usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuarioDnp, string usuarioLogueado)
        {
            return await _serviciosNegocioServicios.EnviarConceptoDireccionTecnicaTramite(tramiteId, usuarioDnp, usuarioLogueado);
        }
        public async Task<List<TramiteModalidadContratacionDto>> ObtenerModalidadesContratacion(int mostrar, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerModalidadesContratacion(mostrar, usuarioDnp);
        }
        public async Task<ActividadPreContractualDto> ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarActividadesCronograma(ModalidadContratacionId, usuarioDnp);
        }
        public async Task<ActividadPreContractualDto> ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerActividadesPrecontractualesProyectoTramite(ModalidadContratacionId, ProyectoId, TramiteId, eliminarActividades, usuarioDnp);
        }

        public async Task<ProductosConstantesVF> ObtenerProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerProductosVigenciaFuturaConstante(Bpin, TramiteId, AnioBase, usuarioDnp);
        }

        public async Task<ProductosCorrientesVF> ObtenerProductosVigenciaFuturaCorriente(string Bpin, int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerProductosVigenciaFuturaCorriente(Bpin, TramiteId, usuarioDnp);
        }

        public async Task<List<TipoDocumentoTramiteDto>>  ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerTipoDocumentoTramitePorNivel(tipoTramiteId, nivelId, rolId, usuarioDnp);
        }


        public async Task<List<DatosUsuarioDto>> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia, string usuarioDNP)
        {
            return await _serviciosNegocioServicios.ObtenerDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia, usuarioDNP);
        }

        public async Task<ModificacionLeyendaDto> ObtenerModificacionLeyenda(int tramiteId, int ProyectoId, string usuarioDNP)
        {
            return await _serviciosNegocioServicios.ObtenerModificacionLeyenda(tramiteId, ProyectoId, usuarioDNP);
        }

        public async Task<string> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarModificacionLeyenda(modificacionLeyendaDto, usuarioDnp);
        }

        public async Task<List<EntidadCatalogoDTDto>> ObtenerListaDirecionesDNP(Guid idEntididad, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerListaDirecionesDNP(idEntididad, usuarioDnp);
        }

        public async Task<List<EntidadCatalogoDTDto>> ObtenerListaSubdirecionesPorParentId(int idEntididadType, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerListaSubdirecionesPorParentId(idEntididadType, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> BorrarFirma(string idUsuario)
        {
            return await _serviciosNegocioServicios.BorrarFirma(idUsuario);
        }

        public async Task<ProyectosCartaDto> ObtenerProyectosCartaTramite(int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerProyectosCartaTramite(TramiteId, usuarioDnp);
        }
        public async Task<DetalleCartaConceptoALDto> ObtenerDetalleCartaAL(int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDetalleCartaAL(TramiteId, usuarioDnp);
        }

        public async Task<int> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerAmpliarDevolucionTramite(ProyectoId, TramiteId, usuarioDnp);
        }

        public async Task<DatosProyectoTramiteDto> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerDatosProyectoConceptoPorInstancia(instanciaId, usuarioDnp);
        }

        public async Task<List<TramiteLiberacionVfDto>> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerLiberacionVigenciasFuturas(ProyectoId, TramiteId, usuarioDnp);
        }

        public async Task<VigenciaFuturaResponse> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuario)
        {
            return await _serviciosNegocioServicios.InsertaAutorizacionVigenciasFuturas(autorizacion, usuario);
        }

        public async Task<VigenciaFuturaResponse> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuario)
        {
            return await _serviciosNegocioServicios.InsertaValoresUtilizadosLiberacionVF(autorizacion, usuario);
        }

        public  async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerListaProyectosFuentes(tramiteId, usuarioDnp);
        }
        public async Task<CartaConcepto> ConsultarCartaConcepto(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ConsultarCartaConcepto(tramiteId, usuarioDnp);
        }


        public async Task<List<EntidadesAsociarComunDto>> obtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.obtenerEntidadAsociarProyecto(InstanciaId, AccionTramiteProyecto, usuarioDnp);
        }

        public async Task<int> ValidacionPeriodoPresidencial(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ValidacionPeriodoPresidencial(tramiteId, usuarioDnp);
        }

        public async Task<string> GuardarMontosTramite(List<ProyectosEnTramiteDto> proyectosEnTramiteDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarMontosTramite(proyectosEnTramiteDto, usuarioDnp);
        }

        public async Task<List<ProyectoJustificacioneDto>> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerPreguntasJustificacionPorProyectos(TramiteId, TipoTramiteId, TipoRolId, IdNivel, usuarioDnp);
        }
        public async Task<List<tramiteVFAsociarproyecto>> ObtenerTramitesVFparaLiberar(string numTramite, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerTramitesVFparaLiberar(numTramite, usuarioDnp);
        }
        public async Task<string> GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarLiberacionVigenciaFutura(liberacionVigenciasFuturasDto, usuarioDnp);
        }

        public async Task<List<ResumenLiberacionVfDto>> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerResumenLiberacionVigenciasFuturas(ProyectoId, TramiteId, usuarioDnp);
        }

        public async Task<ValoresUtilizadosLiberacionVfDto> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuariodnp)
        {
            return await _serviciosNegocioServicios.ObtenerValUtilizadosLiberacionVigenciasFuturas(ProyectoId, TramiteId, usuariodnp);
        }

        public async Task<int> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.TramiteAjusteEnPasoUno(tramiteId, proyectoId, usuarioDnp);
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerListaProyectosFuentesAprobado(tramiteId, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> ActualizarCargueMasivo(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarCargueMasivo(contenido, usuarioDnp);
        }

        public async Task<string> ConsultarCargueExcel(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ConsultarCargueExcel(contenido, usuarioDnp);
        }

        public async Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario)
        {
            return await _serviciosNegocioServicios.InsertaValoresproductosLiberacionVFCorrientes(productosCorrientes, usuario);
        }

        public async Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuario)
        {
            return await _serviciosNegocioServicios.InsertaValoresproductosLiberacionVFConstantes(productosConstantes, usuario);
        }

        public async Task<List<EntidadesAsociarComunDto>> ObtenerEntidadTramite(string numeroTramite, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerEntidadTramite(numeroTramite, usuarioDnp);
        }

        public async Task<VigenciaFuturaResponse> EliminarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto eliminarLiberacionVigenciasFuturasDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.EliminarLiberacionVigenciaFutura(eliminarLiberacionVigenciasFuturasDto, usuarioDnp);
        }
        public async  Task<List<CalendarioPeriodoDto>> ObtenerCalendartioPeriodo(string bpin, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerCalendartioPeriodo(bpin, usuarioDnp);
        }

        public async Task<PresupuestalProyectosAsociadosDto> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId, usuarioDnp);
        }

        public async Task<string> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerPresupuestalProyectosAsociados_Adicion(TramiteId, InstanciaId, usuarioDnp);
        }

        public async Task<string> ObtenerResumenReprogramacionPorVigencia(int TramiteId, Guid InstanciaId, int ProyectoId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerResumenReprogramacionPorVigencia(TramiteId, InstanciaId, ProyectoId, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosReprogramacion(DatosReprogramacionDto Reprogramacion, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarDatosReprogramacion(Reprogramacion, usuarioDnp);
        }

        public async Task<OrigenRecursosDto> GetOrigenRecursosTramite(int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GetOrigenRecursosTramite(TramiteId, usuarioDnp);
        }

        public async Task<VigenciaFuturaResponse> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.SetOrigenRecursosTramite(origenRecurso, usuarioDnp);
        }

        public async Task<SystemConfigurationDto> ConsultarSystemConfiguracion(string VariableKey, string Separador, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ConsultarSystemConfiguracion(VariableKey, Separador, usuarioDnp);
        }

        public async Task<string> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerResumenReprogramacionPorProductoVigencia(InstanciaId, TramiteId, ProyectoId, usuarioDnp);
        }

        public async Task<int> ObtenerModalidadContratacionVigenciasFuturas( int ProyectoId, int TramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerModalidadContratacionVigenciasFuturas( ProyectoId, TramiteId, usuarioDnp);
        }

        public async Task<List<TramiteRVFAutorizacionDto>> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramite, string tipoTramite, string usuario)
        {
            return await _serviciosNegocioServicios.ObtenerAutorizacionesParaReprogramacion(bpin, tramite, tipoTramite, usuario);
        }

        public async Task<string> AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.AsociarAutorizacionRVF(reprogramacionDto, usuarioDnp);
        }

        public async Task<TramiteRVFAutorizacionDto> ObtenerAutorizacionAsociada(int tramiteId, string usuario)
        {
            return await _serviciosNegocioServicios.ObtenerAutorizacionAsociada(tramiteId, usuario);
        }

        public async Task<string> EliminaReprogramacionVF(ReprogramacionDto reprogramacionDto, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.EliminaReprogramacionVF(reprogramacionDto, usuarioDnp);
        }
    }
}