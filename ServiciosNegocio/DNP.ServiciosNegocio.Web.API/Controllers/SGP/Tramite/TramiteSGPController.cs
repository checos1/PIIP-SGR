namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.Tramite
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes.Autorizacion;
    using System;
    using Comunes.Dto;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Swashbuckle.Swagger.Annotations;
    using System.Collections.Generic;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

    public class TramiteSGPController : ApiController
    {
        private readonly ITramiteSGPServicio _tramiteSgpServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;


        public TramiteSGPController(ITramiteSGPServicio tramitesProyectosServicio,
            IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramiteSgpServicio = tramitesProyectosServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramiteSGP/ActualizarEstadoAjusteProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizaEstadoAjusteProyecto([FromUri] string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteSgpServicio.ActualizaEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/EliminarProyectosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var result = await Task.Run(() => _tramiteSgpServicio.EliminarProyectoTramiteNegocio(TramiteId, ProyectoId));


                return Ok(result);
            }
            catch (Exception exception)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = exception.Message
                });
            }
        }

        [Route("api/TramiteSGP/GuardarTramiteInformacionPresupuestal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteInformacionPresupuestal([FromBody] List<TramiteFuentePresupuestalDto> datosTramitesInformacionPresupuestalDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = datosTramitesInformacionPresupuestalDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _tramiteSgpServicio.GuardarTramiteInformacionPresupuestal(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/TramiteSGP/GuardarDatosAdicionSgp")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Incorporacion", typeof(ConvenioDonanteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                          RequestContext.Principal.Identity.GetHashCode().ToString(),
                                          ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                          ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            string usuario = RequestContext.Principal.Identity.Name;

            var parametrosGuardar = new ParametrosGuardarDto<ConvenioDonanteDto>();
            parametrosGuardar.Contenido = objConvenioDonanteDto;

            var result = await Task.Run(() => _tramiteSgpServicio.GuardarDatosAdicionSgp(parametrosGuardar, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/TramiteSGP/eliminarDatosAdicionSgp")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Eiliminar los Datos Incorporacion", typeof(ConvenioDonanteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> eliminarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                          RequestContext.Principal.Identity.GetHashCode().ToString(),
                                          ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                          ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;
            var parametrosGuardar = new ParametrosGuardarDto<ConvenioDonanteDto>();
            parametrosGuardar.Contenido = objConvenioDonanteDto;

            var result = await Task.Run(() => _tramiteSgpServicio.eliminarDatosAdicionSgp(parametrosGuardar, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/TramiteSGP/ObtenerDatosAdicionSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Incorporacion", typeof(ConvenioDonanteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosAdicionSgp(int tramiteId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerDatosIncorporacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _tramiteSgpServicio.ObtenerDatosAdicionSgp(tramiteId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/TramiteSGP/ObtenerListaProyectosFuentes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(List<ProyectoTramiteFuenteDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentes(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteSgpServicio.ObtenerListaProyectosFuentes(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/GuardarFuentesTramiteProyectoAprobacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteSgpServicio.GuardarFuentesTramiteProyectoAprobacion(fuentesTramiteProyectoAprobacion, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerListaProyectosFuentesAprobado")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteSgpServicio.ObtenerListaProyectosFuentesAprobado(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/GuardarTramiteTipoRequisito")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteTipoRequisito([FromBody] List<TramiteRequitoDto> datosProyectoRequisitoDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                            ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = datosProyectoRequisitoDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _tramiteSgpServicio.GuardarTramiteTipoRequisito(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/TramiteSGP/ObtenerProyectoRequisitosPorTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                        RequestContext.Principal.Identity.GetHashCode().ToString(),
                                        ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                        ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var TokenAutorizacion = Request.Headers.Authorization.ToString();
                var result = await Task.Run(() => _tramiteSgpServicio.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, isCDP));

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerContracreditoSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener los proyectos para los contracreditos tramite traslado SGP", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosContracredito([FromBody] ProyectoCreditoParametroDto parametro)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametroCreditos(parametro, false);
            var result = await Task.Run(() => _tramiteSgpServicio.ObtenerProyectosContracredito(parametro.TipoEntidad,
                parametro.IdEntidad,
                parametro.IdFLujo));

            return Ok(result);
        }

        [Route("api/TramiteSGP/ObtenerCreditoSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener los proyectos para los creditos tramite traslado SGP", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosCredito([FromBody] ProyectoCreditoParametroDto parametro)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            ValidacionParametroCreditos(parametro, true);
            var result = await Task.Run(() => _tramiteSgpServicio.ObtenerProyectosCredito(parametro.TipoEntidad,
                parametro.IdEntidad.Value,
                parametro.IdFLujo));

            return Ok(result);
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

        [Route("api/TramiteSGP/ObtenerTiposValorPorEntidadSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTiposValorPorEntidadSgp(int IdEntidad, int IdTipoEntidad)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                        RequestContext.Principal.Identity.GetHashCode().ToString(),
                                        ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                        ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var TokenAutorizacion = Request.Headers.Authorization.ToString();
                var result = await Task.Run(() => _tramiteSgpServicio.ObtenerTiposValorPorEntidad(IdEntidad, IdTipoEntidad));

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}