using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Formulario;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Linq;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Formulario
{
    public class RegionalizacionProyectoController : ApiController
    {
        private readonly IRegionalizacionProyectoServicios _regionalizacionServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public RegionalizacionProyectoController(IRegionalizacionProyectoServicios regionalizacionServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _regionalizacionServicios = regionalizacionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/RegionalizacionProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion del poryecto detallando su regionalización", typeof(RegionalizacionProyectoDto))]
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
                                                               AppSettings["consultarRegionalizacion"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametro(bpin, Request);

                var result =
                    await Task.Run(() => _regionalizacionServicios.ObtenerRegionalizacion(new ParametrosConsultaDto
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
                                                                                                           First())
                                                                                          }));


                if (result?.Vigencias != null) return Ok(result);

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

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (peticion.Headers.Contains("piip-idInstanciaFlujo"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idInstanciaFlujo").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idInstanciaFlujo").First(), out var outGuid))
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

                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idAccion").First(), out var outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idAccion"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idAccion"));
            }
        }

        [Route("api/RegionalizacionProyecto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Información del proyecto detallando su regionalización con información dummy", typeof(RegionalizacionProyectoDto))]
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
                                                           ConfigurationManager.AppSettings["previewRegionalizacion"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _regionalizacionServicios.ObtenerRegionalizacionPreview());

                return Ok(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/RegionalizacionProyecto/Temporal")]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(RegionalizacionProyectoDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postRegionalizacionTemporal"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _regionalizacionServicios.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.
                                                        Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _regionalizacionServicios.Guardar(parametrosGuardar, parametrosAuditoria, true));

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

        [Route("api/RegionalizacionProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo([FromBody]RegionalizacionProyectoDto proyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postRegionalizacionDefinitivo"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var parametrosGuardar = _regionalizacionServicios.ConstruirParametrosGuardado(Request, proyecto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.
                                                        Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _regionalizacionServicios.Guardar(parametrosGuardar, parametrosAuditoria, false));

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