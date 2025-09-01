// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Dto.ObjetosNegocio;
    using Comunes.Excepciones;
    using Dominio.Dto.Proyectos;
    using Servicios.Interfaces.Proyectos;
    using Swashbuckle.Swagger.Annotations;
    using System;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using Newtonsoft.Json.Linq;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;

    public class ProyectoController : ApiController
    {
        private readonly IProyectoServicio _proyectoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ProyectoController(IProyectoServicio proyectoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _proyectoServicio = proyectoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Proyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna un proyecto por bpin", typeof(ProyectoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarProyecto"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin);

            var tokenAutorizacion = Request.Headers.Authorization.Parameter;

            var result = await Task.Run(() => _proyectoServicio.ObtenerProyecto(bpin, tokenAutorizacion));

            return Responder(result);
        }

        [Route("api/Proyecto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna un proyecto dummy", typeof(ProyectoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["previewProyecto"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectoPreview());

            return Responder(result);
        }

        [Route("api/InsertarAuditoriaEntidad")]
        [SwaggerResponse(HttpStatusCode.OK, "Inserta un nuevo cambio de entidad del proyecto actual", typeof(AuditoriaEntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> InsertarAuditoriaEntidad([FromBody] JObject values)
        {
            try
            {

                var auditoria = values["auditoria"].ToObject<AuditoriaEntidadDto>();
                var resultado = await Task.Run(() => _proyectoServicio.InsertarAuditoriaEntidad(auditoria));
                return Ok(new
                {
                    Datos = resultado.Exito ? (short)resultado.Registros.First() : 0,
                    EsExcepcion = !resultado.Exito,
                    MensajeExcepcion = !resultado.Exito ? resultado.Mensaje : String.Empty
                });
            }
            catch (Exception exception)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    MensajeExcepcion = $"Proyecto.InsertarAuditoriaEntidad => ${exception.Message}\n{exception.InnerException?.Message ?? string.Empty}"
                });
            }
        }

        [Route("api/ObtenerAuditoriaEntidad")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene el historial de cambio de entidades del proyecto actual", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAuditoriaEntidad(int proyectoId)
        {
            try
            {
                var resultado = await Task.Run(() => _proyectoServicio.ObtenerAuditoriaEntidad(proyectoId));
                return Ok(new
                {
                    Datos = resultado.Exito ? resultado.Registros : new List<object>(),
                    EsExcepcion = !resultado.Exito,
                    MensajeExcepcion = !resultado.Exito ? resultado.Mensaje : String.Empty
                });
            }
            catch (Exception exception)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    MensajeExcepcion = $"Proyecto.ObtenerAuditoriaEntidad => ${exception.Message}\n{exception.InnerException?.Message ?? string.Empty}"
                });
            }
        }

        [Route("api/Proyectos")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectoEntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosPorEntidadesYEstados(ParametrosProyectosDto entidadesEstados)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                   ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(entidadesEstados.IdsEntidades, entidadesEstados.NombresEstadosProyectos);

            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = entidadesEstados.IdsEntidades,
                NombresEstadosProyectos = entidadesEstados.NombresEstadosProyectos,
                TokenAutorizacion = Request.Headers.Authorization.Parameter
            };

            var result = await Task.Run(() => _proyectoServicio.ConsultarProyectosPorEntidadesYEstados(parametros));

            return Responder(result);
        }

        [Route("api/ProyectosPriorizar")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectoPriorizarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosPriorizar(String IdUsuarioDNP)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            //ValidacionParametro(IdUsuarioDNP);

            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectosPriorizar(IdUsuarioDNP));

            return Responder(result);
        }

        [Route("api/ProyectosPorBPINs")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectoEntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(bpins);
            bpins.TokenAutorizacion = Request.Headers.Authorization.Parameter;

            var result = await Task.Run(() => _proyectoServicio.ConsultarProyectosPorBPINs(bpins));

            return Responder(result);
        }

        [Route("api/EntidadesPorIds")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna entidades", typeof(EntidadDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarEntidadesPorIds([FromUri] List<string> idsEntidades)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(idsEntidades);

            var result = await Task.Run(() => _proyectoServicio.ConsultarEntidadesPorIds(idsEntidades));

            return Responder(result);
        }

        [Route("api/EntidadesPorIds")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna entidades", typeof(EntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarEntidadesPorIdss(List<string> idsEntidades)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(idsEntidades);

            var result = await Task.Run(() => _proyectoServicio.ConsultarEntidadesPorIds(idsEntidades));

            return Responder(result);
        }

        [Route("api/ProyectosPorIds")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectoEntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosPorIds(List<int> ids)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.ConsultarProyectosPorIds(ids));

            return Responder(result);
        }

        [Route("api/ObtenerCRType")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna CRtype lista", typeof(CrTypeDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCRType()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.ObtenerCRType());

            return Responder(result);
        }

        [Route("api/ObtenerFase")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Fase lista", typeof(FaseDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFase()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.ObtenerFase());

            return Responder(result);
        }

        [Route("api/MantenimientoMatrizFlujo")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar nueva Matriz de Flujo", typeof(MatrizEntidadDestinoAccionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            foreach (var flujo in flujos)
            {
                flujo.Creado = DateTime.Now;
                flujo.CreadoPor = RequestContext.Principal.Identity.Name;
                flujo.Modificado = DateTime.Now;
                flujo.ModificadoPor = RequestContext.Principal.Identity.Name;
            }

            var result = await Task.Run(() => _proyectoServicio.MantenimientoMatrizFlujo(flujos));

            return Ok(result);
        }

        [Route("api/ObtenerMatrizFlujo")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Matriz de Flujo", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerMatrizFlujo(int entidadResponsableId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.ObtenerMatrizFlujo(entidadResponsableId));

            return Ok(result);
        }

        [Route("api/ObtenerContracredito")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener los proyectos para los contracreditos", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosContracredito([FromBody] ProyectoCreditoParametroDto parametro)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametroCreditos(parametro, false);
            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectosContracredito(parametro.TipoEntidad, 
                parametro.IdEntidad, 
                parametro.IdFLujo, 
                parametro.IdEntidadFiltro, 
                parametro.BPIN, 
                parametro.NombreProyecto));

            return Ok(result);
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <returns>string</returns> 
        [Route("api/ValidacionDevolucionPaso")]
        [SwaggerResponse(HttpStatusCode.OK, "Validacion previa a la devolución de un paso", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["ValidacionDevolucionPaso"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _proyectoServicio.ValidacionDevolucionPaso(instanciaId, accionId, accionDevolucionId, RequestContext.Principal.Identity.Name));

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ObtenerCredito")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener los proyectos para los creditos", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosCredito([FromBody] ProyectoCreditoParametroDto parametro)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            ValidacionParametroCreditos(parametro, true);
            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectosCredito(parametro.TipoEntidad,
                parametro.IdEntidad.Value,
                parametro.IdFLujo,
                parametro.BPIN,
                parametro.NombreProyecto));

            return Ok(result);
        }

        private void ValidacionParametro(string bpin)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            // ReSharper disable once UnusedVariable
            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));
        }

        private void ValidarParametros(BPINsProyectosDto BPINs)
        {
            if (BPINs == null || BPINs.BPINs == null || BPINs.BPINs.Count == 0 || BPINs.BPINs.Where(b => string.IsNullOrEmpty(b) == true).Count() > 0)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(BPINs)));

            // ReSharper disable once UnusedVariable
            if (BPINs.BPINs.Where(b => !long.TryParse(b, out var outBpin) || b.Length > 100).Count() > 0)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(BPINs)));
        }

        private void ValidarParametros(List<string> idsEntidades)
        {
            if (idsEntidades == null || idsEntidades.Count == 0 || idsEntidades.Where(e => string.IsNullOrEmpty(e)).Count() > 0)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(idsEntidades)));

            if (idsEntidades.Where(e => long.TryParse(e, out var outIdEnt) == false).Count() > 0)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametrosNoValidos, nameof(idsEntidades)));
        }

        private void ValidarParametros(List<int> idEntidades, List<string> nombresEstadosProyectos)
        {
            if (idEntidades == null || idEntidades.Count == 0)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(idEntidades)));

            if (nombresEstadosProyectos == null || nombresEstadosProyectos.Count == 0)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(nombresEstadosProyectos)));
        }

        private IHttpActionResult Responder(List<ProyectoEntidadDto> listaProyecto)
        {
            return listaProyecto != null ? Ok(listaProyecto) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(List<ProyectoPriorizarDto> listaProyecto)
        {
            return listaProyecto != null ? Ok(listaProyecto) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(List<CrTypeDto> listaCRType)
        {
            return listaCRType != null ? Ok(listaCRType) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(List<FaseDto> listaFase)
        {
            return listaFase != null ? Ok(listaFase) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(List<EntidadDto> listaEntidades)
        {
            return listaEntidades != null ? Ok(listaEntidades) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(ProyectoDto proyecto)
        {
            return proyecto != null ? Ok(proyecto) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(List<ProyectoDto> listaProyecto)
        {
            return listaProyecto != null ? Ok(listaProyecto) : CrearRespuestaNoFound();
        }

        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        private void ValidacionParametroCreditos(ProyectoCreditoParametroDto parametros, bool esCredito)
        {
            if (parametros.IdFLujo.Equals(Guid.Empty) || parametros.IdFLujo.Equals(null))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(parametros.IdFLujo)));


            if (string.IsNullOrEmpty(parametros.TipoEntidad))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(parametros.TipoEntidad)));

            //El tipo de entidad solo es obligatorio para los creditos
            if (esCredito)
            {
                if (!parametros.IdEntidad.HasValue)
                    throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(parametros.IdEntidad)));
            }
        }

        [Route("api/ObtenerProyectoConpes")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Proyecto Conpes", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoConpes(int proyectoid, Guid InstanciaId, string GuiMacroproceso,string NivelId, string FlujoId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectoConpes(proyectoid, InstanciaId, GuiMacroproceso, NivelId, FlujoId));

            return Ok(result);
        }

        [Route("api/ActualizarHorizonteProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarHorizonteProyecto([FromBody] HorizonteProyectoDto DatosHorizonteProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _proyectoServicio.ActualizarHorizonte(DatosHorizonteProyecto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };


                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/AdicionarProyectoConpes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AdicionarProyectoConpes([FromBody] CapituloConpes Conpes)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _proyectoServicio.AdicionarProyectoConpes(Conpes, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/EliminarProyectoConpes")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Proyecto Conpes", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> EliminarProyectoConpes(int proyectoid, int conpesid)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.EliminarProyectoConpes(proyectoid, conpesid));

            return Ok(result);
        }

        [Route("api/ObtenerResumenObjetivosProductosActividades")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Objetivos Productos Actividades", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenObjetivosProductosActividades(string bpin)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerResumenObjetivosProductosActividades(bpin));

            return Ok(result);
        }

        [Route("api/GuardarCostoActividades")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCostoActividades(ProductoAjusteDto producto)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _proyectoServicio.GuardarAjusteCostoActividades(producto, RequestContext.Principal.Identity.Name));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/AgregarEntregable")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarEntregable(AgregarEntregable[] entregables)
        {
            try
            {
                await Task.Run(() => _proyectoServicio.AgregarEntregable(entregables, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/EliminarEntregable")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Entregable / Actividad", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarEntregable(EntregablesActividadesDto entregable)
        {
            try
            {
                await Task.Run(() => _proyectoServicio.EliminarEntregable(entregable));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ObtenerResumenObjetivosProductosActividadesJustificacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Objetivos Productos Actividades Justificación", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerResumenObjetivosProductosActividadesJustificacion(bpin));

            return Ok(result);
        }


        [Route("api/ObtenerJustificacionLocalizacionProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Localizaciones en firme y cambios", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<LocalizacionJustificacionProyectoDto> ObtenerJustificacionLocalizacionProyecto(int idProyecto)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerJustificacionLocalizacionProyecto(idProyecto));

            return result;
        }

        [Route("api/ObtenerInstanciaProyectoTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener la instancia de los proyectos asociados a un tramite", typeof(List<ProyectoInstanciaDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerInstanciaProyectoTramite(InstanciaId,BPIN));

            return Ok(result);
        }

        [Route("api/ObtenerProyectosBeneficiarios")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Objetivos Productos Actividades", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiarios(string bpin)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectosBeneficiarios(bpin));

            return Ok(result);
        }

        [Route("api/ObtenerProyectosBeneficiariosDetalle")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerProyectosBeneficiarios Detalle", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiariosDetalle(string json)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerProyectosBeneficiariosDetalle(json));

            return Ok(result);
        }

        [Route("api/ObtenerJustificacionProyectosBeneficiarios")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Objetivos Productos Actividades", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerJustificacionProyectosBeneficiarios(string bpin)
        {
            var result = await Task.Run(() => _proyectoServicio.ObtenerJustificacionProyectosBeneficiarios(bpin));

            return Ok(result);
        }

        [Route("api/GuardarBeneficiarioTotales")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario)
        {
            try
            { 
                await Task.Run(() => _proyectoServicio.GuardarBeneficiarioTotales(beneficiario, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ObtenerMatrizEntidadDestino")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta ObtenerMatrizEntidadDestino", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto)
        {
            try
            {
                return Ok(await Task.Run(() => _proyectoServicio.ObtenerMatrizEntidadDestino(dto, RequestContext.Principal.Identity.Name)));
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ActualizarMatrizEntidadDestino")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta ObtenerMatrizEntidadDestino", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto)
        {
            try
            {
                return Ok(await Task.Run(() => _proyectoServicio.ActualizarMatrizEntidadDestino(dto, RequestContext.Principal.Identity.Name)));
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/GuardarBeneficiarioProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario)
        {
            try
            {
                await Task.Run(() => _proyectoServicio.GuardarBeneficiarioProducto(beneficiario, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/GuardarBeneficiarioProductoLocalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario)
        {
            try
            {
                await Task.Run(() => _proyectoServicio.GuardarBeneficiarioProductoLocalizacion(beneficiario, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/GuardarBeneficiarioProductoLocalizacionCaracterizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario)
        {
            try
            {
                await Task.Run(() => _proyectoServicio.GuardarBeneficiarioProductoLocalizacionCaracterizacion(beneficiario, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ObtenerCategoriasSubcategoriasPorPadre")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Categorias y Subcategorias por padre", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategoriasSubcategorias_JSON(int padreId, Nullable<int> entidadId, int esCategoria, int esGruposEtnicos)
        {
            var result = await Task.Run(() => _proyectoServicio.GetCategoriasSubcategorias_JSON(padreId,entidadId,esCategoria,esGruposEtnicos));

            return Ok(result);
        }

        [Route("api/ProyectosASeleccionar")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectoEntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosASeleccionar(ParametrosProyectosDto entidadesEstados)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarConsultarProyectosASeleccionar"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(entidadesEstados.IdsEntidades, entidadesEstados.NombresEstadosProyectos);

            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = entidadesEstados.IdsEntidades,
                NombresEstadosProyectos = entidadesEstados.NombresEstadosProyectos,
                TokenAutorizacion = Request.Headers.Authorization.Parameter,
                flujoid = entidadesEstados.flujoid,
                IdUsuarioDNP = entidadesEstados.IdUsuarioDNP,
                tipoTramiteId = entidadesEstados.tipoTramiteId,
                tipoEntidad = entidadesEstados.tipoEntidad
            };

            var result = await Task.Run(() => _proyectoServicio.ConsultarProyectosASeleccionar(parametros));

            return Responder(result);
        }

        [Route("api/GuardarReprogramacionPorProductoVigencia")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar o actualizar valores producto vigencia", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                              ConfigurationManager.AppSettings["ConsultarConsultarProyectosASeleccionar"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _proyectoServicio.GuardarReprogramacionPorProductoVigencia(reprogramacionValores, RequestContext.Principal.Identity.Name));

            return Ok(result);
        }
        [Route("api/DocumentosProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna documentos proyecto", typeof(DocumentosDto))]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentosProyecto(FiltroDocumentosDto filtroDocumentos)
        {

            var result = await Task.Run(() => _proyectoServicio.ObtenerDocumentosProyecto(filtroDocumentos));

            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/PlanNacionalDesarrollo")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna PND", typeof(DocumentosDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPND(int idProyecto)
        {

            var result = await Task.Run(() => _proyectoServicio.ObtenerPND(idProyecto));

            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }
    }


}