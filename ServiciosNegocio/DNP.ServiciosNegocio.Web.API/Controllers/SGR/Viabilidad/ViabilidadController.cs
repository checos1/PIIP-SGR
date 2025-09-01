using System.Collections.Generic;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;


namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.Viabilidad
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Net.Http;

    [Route("api/[controller]")]

    public class ViabilidadController : ApiController
    {
        private readonly IViabilidadServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ViabilidadController(IViabilidadServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }


        [Route("SGR/Viabilidad/LeerInformacionGeneral")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee la información de un proyecto para el proceso de viabilidad.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            try
            {
                //var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                            ConfigurationManager.AppSettings["SGR_Viabilidad_LeerInformacionGeneral"]);

                //if (!respuestaAutorizacion.IsSuccessStatusCode)
                //    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode));
                if (result.Id > 0) return Ok(result);

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

        [Route("SGR/Viabilidad/LeerParametricas")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee las paramétricas utilizadas en el proceso de viabilidad.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Viabilidad_LeerParametricas(int proyectoId, System.Guid nivelId)
        {
            try
            {
                //var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                            ConfigurationManager.AppSettings["SGR_Viabilidad_LeerParametricas"]);

                //if (!respuestaAutorizacion.IsSuccessStatusCode)
                //    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_LeerParametricas(proyectoId, nivelId));
                if (!string.IsNullOrEmpty(result)) return Ok(result);

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

        [Route("SGR/Viabilidad/GuardarInformacionBasica")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_GuardarInformacionBasica(string json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_Viabilidad_GuardarInformacionBasica"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_GuardarInformacionBasica(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Viabilidad/FirmaUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_FirmarUsuario(string json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_Viabilidad_FirmarViabilidad"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_FirmarUsuario(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Viabilidad/EliminarFirmaUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_EliminarFirmaUsuario(string json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_Viabilidad_FirmarViabilidad"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_EliminarFirmaUsuario(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Viabilidad/ObtenerPuntajeProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene el puntaje SEP de un proyecto en el proceso de viabilidad OCAD Paz.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Viabilidad_ObtenerPuntajeProyecto(Guid instanciaId, int entidadId)
        {
            try
            {
                //var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                            ConfigurationManager.AppSettings[""]);
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_ObtenerPuntajeProyecto(instanciaId, entidadId));
                if (!string.IsNullOrEmpty(result)) return Ok(result);

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

        [Route("SGR/Viabilidad/GuardarPuntajeProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_GuardarPuntajeProyecto([FromBody] JValue json)
        {
            try
            {
                //var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                            ConfigurationManager.AppSettings[""]);
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_GuardarPuntajeProyecto(json.ToString(), RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}