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
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
    using System;
    using System.Net.Http;

    [Route("api/[controller]")]

    public class CTUSController : ApiController
    {
        private readonly ICTUSServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CTUSController(ICTUSServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/CTUS/LeerProyectoCtusConcepto")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_LeerProyectoCtusConcepto"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/CTUS/GuardarProyectoCtusConcepto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_GuardarProyectoCtusConcepto"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_CTUS_GuardarProyectoCtusConcepto(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/CTUS/GuardarAsignacionUsuarioEncargado")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_GuardarAsignacionUsuarioEncargado"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_CTUS_GuardarAsignacionUsuarioEncargado(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/CTUS/LeerProyectoCtusUsuarioEncargado")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_LeerProyectoCtusUsuarioEncargado"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_CTUS_LeerProyectoCtusUsuarioEncargado(proyectoCtusId, instanciaId));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/CTUS/GuardarResultadoConceptoCtus")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_GuardarResultadoConceptoCtus"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_CTUS_GuardarResultadoConceptoCtus(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/CTUS/LeerRolDirectorProyectoCtus")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_CTUS_LeerRolDirectorProyectoCtus(int proyectoId, Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_LeerRolDirectorProyectoCtus"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_CTUS_LeerRolDirectorProyectoCtus(proyectoId, instanciaId));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>       
        /// <param name="user"></param> 
        /// <returns>Json</returns> 
        [Route("SGR/CTUS/ActualizarEntidadAdscritaCTUS")]
        [SwaggerResponse(HttpStatusCode.OK, "Actualizar Entidad Adscrita", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string user)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                        RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                        ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                        ConfigurationManager.AppSettings["SGR_Proyectos_ActualizarEntidadAdscrita"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, tipo, user));

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

        [Route("SGR/CTUS/ValidarInstanciaCTUSNoFinalizada")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidarInstanciaCTUSNoFinalizada(int idProyecto)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTUS_ValidarInstanciaCTUSNoFinalizada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.ValidarInstanciaCTUSNoFinalizada(idProyecto));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}