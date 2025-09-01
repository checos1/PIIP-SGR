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
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using Swashbuckle.Swagger.Annotations;
    public class CuantificacionLocalizacionController : ApiController
    {

        private readonly ICuantificacionLocalizacionServicios _cuantificacionLocalizacionServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CuantificacionLocalizacionController(ICuantificacionLocalizacionServicios cuantificacionLocalizacionServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _cuantificacionLocalizacionServicios = cuantificacionLocalizacionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }


        [Route("api/CuantificacionLocalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion de la cuantificación de la localización de la población", typeof(PoblacionDto))]
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
                                                           ConfigurationManager.AppSettings["consultarCuantificacionLocalizacion"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametro(bpin, Request);

                var result =
                    await Task.Run(() => _cuantificacionLocalizacionServicios.ObtenerCuantificacionLocalizacion(new ParametrosConsultaDto
                    {
                        Bpin =
                                                                                          bpin,
                        AccionId
                                                                                          = new
                                                                                              Guid(Request.
                                                                                                   Headers.
                                                                                                   GetValues("piip-idAccion").
                                                                                                   First()),
                        InstanciaId
                                                                                          = new
                                                                                              Guid(Request.
                                                                                                   Headers.
                                                                                                   GetValues("piip-idInstanciaFlujo").
                                                                                                   First())
                    }));

                if (result != null) return Ok<PoblacionDto>(result);

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
        }


        [Route("api/CuantificacionLocalizacion/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion de la cuantificacion de la localizacion dummy", typeof(PoblacionDto))]
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
                                                           ConfigurationManager.AppSettings["previewCuantificacionLocalizacion"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _cuantificacionLocalizacionServicios.ObtenerCuantificacionLocalizacionPreview());

                return Ok<PoblacionDto>(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }



        [Route("api/CuantificacionLocalizacion/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado temporal", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(PoblacionDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["postCuantificacionLocalizacionTemporal"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _cuantificacionLocalizacionServicios.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                              Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _cuantificacionLocalizacionServicios.Guardar(parametrosGuardar, parametrosAuditoria, true)));

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




        [Route("api/CuantificacionLocalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo([FromBody]PoblacionDto proyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postCuantificacionLocalizacionDefinitivo"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var parametrosGuardar = _cuantificacionLocalizacionServicios.ConstruirParametrosGuardado(Request, proyecto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                              Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _cuantificacionLocalizacionServicios.Guardar(parametrosGuardar, parametrosAuditoria, false)));

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
