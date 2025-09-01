using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
using DNP.ServiciosWBS.Servicios.Interfaces;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosWBS.Web.API.Controllers
{
    public class AjustesRegionalizacionMetasyRecursosController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IAjusteRegionalizacionMetasyRecursosServicios _ajusteRegionalizacionMetasyRecursosServicios;

        public AjustesRegionalizacionMetasyRecursosController(IAutorizacionUtilidades autorizacionUtilidades, IAjusteRegionalizacionMetasyRecursosServicios ajusteRegionalizacionMetasyRecursosServicios)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _ajusteRegionalizacionMetasyRecursosServicios = ajusteRegionalizacionMetasyRecursosServicios;
        }
        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_RegMetasRecursosDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Productos Regionalización Metas y Recursos Ajuste", typeof(AjusteRegMetasRecursosDto))]
        [System.Web.Http.HttpGet]
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
                                                               AppSettings["consultarRegionalizacionMetasyRecursosAjuste"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametro(bpin, Request);

                var result =
                    await Task.Run(() => _ajusteRegionalizacionMetasyRecursosServicios.ObtenerRegionalizacionMetasyRecursosAjusteServicios(new ParametrosConsultaDto
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


                if (result?.Productos != null) return Ok<AjusteRegMetasRecursosDto>(result);

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

        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_RegMetasRecursosDto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Productos Regionalización Metas y Recursos Ajuste dummy", typeof(AjusteRegMetasRecursosDto))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["previewRegionalizacionMetasyRecursosAjuste"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _ajusteRegionalizacionMetasyRecursosServicios.ObtenerRegionalizacionMetasyRecursosServiciosAjustePreview());

                return Ok<AjusteRegMetasRecursosDto>(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_RegMetasRecursosDto/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Productos devuelta por guardado temporal Ajuste", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Temporal(AjusteRegMetasRecursosDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postRegionalizacionMetasyRecursosAjuste"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _ajusteRegionalizacionMetasyRecursosServicios.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                                                        Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _ajusteRegionalizacionMetasyRecursosServicios.Guardar(parametrosGuardar, parametrosAuditoria, true)));

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

        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_RegMetasRecursosDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Productos devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Definitivo([FromBody] AjusteRegMetasRecursosDto proyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postDefinitivoRegionalizacionMetasyRecursosAjuste"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var parametrosGuardar = _ajusteRegionalizacionMetasyRecursosServicios.ConstruirParametrosGuardado(Request, proyecto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                                                        Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _ajusteRegionalizacionMetasyRecursosServicios.Guardar(parametrosGuardar, parametrosAuditoria, false)));

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