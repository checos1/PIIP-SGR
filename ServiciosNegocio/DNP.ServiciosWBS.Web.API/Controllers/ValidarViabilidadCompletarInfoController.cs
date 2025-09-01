namespace DNP.ServiciosWBS.Web.API.Controllers
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;
    using Swashbuckle.Swagger.Annotations;

    public class ValidarViabilidadCompletarInfoController : ApiController
    {
        private readonly IValidarViabilidadCompletarInfoServicios _validarViabilidadCompletarInfoServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ValidarViabilidadCompletarInfoController(IValidarViabilidadCompletarInfoServicios validarViabilidadCompletarInfoServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _validarViabilidadCompletarInfoServicios = validarViabilidadCompletarInfoServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/ValidarViabilidadCompletarInfo")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion del poryecto detallando su focalización", typeof(ValidarViabilidadCompletarInfoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.
                                                               AppSettings["consultarValidarViabilidadCompletarInfo"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametro(bpin, Request);

                var result =
                    await Task.Run(() => _validarViabilidadCompletarInfoServicios.ObtenerValidarViabilidadCompletarInfo(new ParametrosConsultaDto
                                                                                          {
                                                                                              Bpin
                                                                                                  =
                                                                                                  bpin,
                                                                                              AccionId
                                                                                                  =
                                                                                                  new
                                                                                                      Guid(Request.
                                                                                                           Headers.
                                                                                                           GetValues("piip-idAccion").
                                                                                                           First()),
                                                                                              InstanciaId
                                                                                                  = new
                                                                                                      Guid(Request.
                                                                                                           Headers.
                                                                                                           GetValues("piip-idInstanciaFlujo").
                                                                                                           First()),
                                                                                             Usuario = RequestContext.
                                                                                                        Principal.Identity.Name
                    }));


                if (result?.Mensaje != null) return Ok<ValidarViabilidadCompletarInfoDto>(result);

                var respuestaHttp = new HttpResponseMessage()
                                    {
                                        StatusCode = HttpStatusCode.NotFound,
                                        ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                                    };

                return ResponseMessage(respuestaHttp);

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

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametro(string bpin, HttpRequestMessage peticion)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            long outBpin;
            if (!long.TryParse(bpin, out outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (peticion.Headers.Contains("piip-idInstanciaFlujo"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idInstanciaFlujo").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

                Guid outGuid = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idInstanciaFlujo").First(), out outGuid))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idInstanciaFlujo"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                          "piip-idInstanciaFlujo"));
            }


            if (peticion.Headers.Contains("piip-idAccion"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idAccion").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idAccion"));

                Guid outGuidAccionId = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idAccion").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idAccion"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idAccion"));
            }

            if (peticion.Headers.Contains("piip-idFormulario"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idFormulario").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idFormulario"));

                Guid outGuidAccionId = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idFormulario").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idFormulario"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idFormulario"));
            }
        }

        [Route("api/ValidarViabilidadCompletarInfo/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Información del proyecto detallando su focalización con información dummy", typeof(ValidarViabilidadCompletarInfoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["previewValidarViabilidadCompletarInfo"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _validarViabilidadCompletarInfoServicios.ObtenerValidarViabilidadCompletarInfoPreview());

                return Ok<ValidarViabilidadCompletarInfoDto>(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ValidarViabilidadCompletarInfo/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado temporal", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(ValidarViabilidadCompletarInfoDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postValidarViabilidadCompletarInfoTemporal"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.
                                                        Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                //await Task.Run((Action)(() => _validarViabilidadCompletarInfoServicios.Guardar(parametrosGuardar, parametrosAuditoria, true)));
                await Task.Run(() =>
                {
                    return true;
                });

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

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

        [Route("api/ValidarViabilidadCompletarInfo")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo([FromBody]ValidarViabilidadCompletarInfoDto proyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postValidarViabilidadCompletarInfoDefinitivo"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var parametrosGuardar = _validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(Request, proyecto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.
                                                        Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run((Action) (() => _validarViabilidadCompletarInfoServicios.Guardar(parametrosGuardar, parametrosAuditoria, false)));

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

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
    }
}