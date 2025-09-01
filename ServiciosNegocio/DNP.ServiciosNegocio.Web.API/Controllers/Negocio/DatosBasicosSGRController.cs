using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public class DatosBasicosSGRController : ApiController
    {
        private readonly IDatosBasicosSGRServicio _datosBasicosSGRServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public DatosBasicosSGRController(IDatosBasicosSGRServicio datosBasicosSGRServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosBasicosSGRServicios = datosBasicosSGRServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/DatosBasicosSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los datos basicos", typeof(DatosBasicosSGRDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarDatosBasicosSGR"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _datosBasicosSGRServicios.ObtenerDatosBasicosSGR(
                                                                                                             new ParametrosConsultaDto
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
                                                                                                             }

                                                                                                             ));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametro(string bpin, HttpRequestMessage peticion)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (peticion.Headers.Contains("piip-idFormulario"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idFormulario").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idFormulario"));

                Guid outGuidAccionId = Guid.Empty;

                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idFormulario").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idFormulario"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idFormulario"));
            }
        }

        [Route("api/DatosBasicosSGR/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los datos basicos dummy", typeof(DatosBasicosSGRDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["previewDatosBasicosSGR"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _datosBasicosSGRServicios.ObtenerDatosBasicosSGRPreview());
            return Ok(result);
        }


        [Route("api/DatosBasicosSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(DatosBasicosSGRDto datosBasicosSGRDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postDatosBasicosSGR"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _datosBasicosSGRServicios.ConstruirParametrosGuardado(Request, datosBasicosSGRDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _datosBasicosSGRServicios.Guardar(parametrosGuardar, parametrosAuditoria, false));

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

        [Route("api/DatosBasicosSGR/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(DatosBasicosSGRDto datosBasicosSGRDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), 
                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postDatosBasicosSGRTemporal"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _datosBasicosSGRServicios.ConstruirParametrosGuardado(Request, datosBasicosSGRDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _datosBasicosSGRServicios.Guardar(parametrosGuardar, parametrosAuditoria, true));

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
    }
}