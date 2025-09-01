using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.Transversales
{
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using System;
    using System.Net.Http;
    
    [Route("api/[controller]")]
    public class TransversalController : ApiController
    {
        private readonly ITransversalServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TransversalController(ITransversalServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/Transversal/LeerParametro")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee un parámetro de base de datos.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_LeerParametro(string parametro)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_LeerParametro"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Transversal_LeerParametro(parametro));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/LeerListaParametros")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee una lista de parámetros para almacenarlo en variables de sessionStorage", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerListaParametros()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_LeerListaParametros"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerListaParametros());
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/ObtenerTipoDocumentoSoporte")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los documentos soporte.", typeof(TipoDocumentoSoporteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ObtenerTipoDocumentoSoporte([FromUri] int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerTipoDocumentoSoporte"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Transversal_ObtenerTipoDocumentoSoporte(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/ObtenerListaTipoDocumentoSoporte")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los documentos soporte.", typeof(TipoDocumentoSoporteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ObtenerListaTipoDocumentoSoporte([FromUri] int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerTipoDocumentoSoporte"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Transversal_ObtenerListaTipoDocumentoSoporte(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/ObtenerConfiguracionReportes")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee la configuración de reportes por flujo.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerConfiguracionReportes"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Transversal_ObtenerConfiguracionReportes(instanciaId));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/AutorizacionAccionesPorInstanciaSubFlujoOCADPaz")]
        [SwaggerResponse(HttpStatusCode.OK, "Cambiar estado subflujo OCADPaz", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["SGR_Proyectos_GuardarProyectoCtus"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                await Task.Run(() => _datosServicios.AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(instanciaId, RolId, RequestContext.Principal.Identity.Name));

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

        [Route("SGR/Transversal/ValidacionOCADPaz")]
        [SwaggerResponse(HttpStatusCode.OK, "Cambiar estado subflujo OCADPaz", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerTipoDocumentoSoporte"]);

                var result = Task.Run(() => _datosServicios.SGR_Transversal_ValidacionOCADPaz(proyectoId, nivelId, instanciaId, flujoId));

                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/LeerUsuariosNotificacionViabilidad")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los usuarios que participaron en el proceso de viabilidad SGR.", typeof(Guid))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ObtenerUsuariosNotificacionViabilidad(Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_LeerUsuariosNotificacionViabilidad"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Transversal_ObtenerUsuariosNotificacionViabilidad(instanciaId));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        [Route("SGR/Transversal/LeerUsuariosNotificacionInvolucrados")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los usuarios involucrados en el proceso de viabilidad SGR.", typeof(Guid))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ObtenerInformacionNotificacionInvolucrados(Guid instanciaId, string usuarioFirma)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_LeerUsuariosNotificarInvolucrados"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Transversal_ObtenerInformacionNotificacionInvolucrados(instanciaId, usuarioFirma));
                if (result != null) return Ok(result);

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
    }
}