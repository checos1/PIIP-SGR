using System.Collections.Generic;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;


namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.Viabilidad
{
    using DNP.ServiciosNegocio.Comunes.Dto;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
    using System;
    using System.Net.Http;

    [Route("api/[controller]")]

    public class ViabilidadSGPController : ApiController
    {
        private readonly IViabilidadSGPServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ViabilidadSGPController(IViabilidadSGPServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGP/Transversal/LeerParametro")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee un parámetro de base de datos.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPTransversalLeerParametro(string parametro)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGPTransversalLeerParametro"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPTransversalLeerParametro(parametro));
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
        [Route("SGP/Viabilidad/SGPViabilidadLeerInformacionGeneral")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee la información de un proyecto para el proceso de viabilidad.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPViabilidadLeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGPViabilidadLeerInformacionGeneral"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPViabilidadLeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode));
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

        [Route("SGP/Viabilidad/LeerParametricas")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee las paramétricas utilizadas en el proceso de viabilidad.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGPViabilidadLeerParametricas"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPViabilidadLeerParametricas(proyectoId, nivelId));
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

        [Route("SGP/Viabilidad/GuardarInformacionBasica")]
        [HttpPost]
        public async Task<IHttpActionResult> SGPViabilidadGuardarInformacionBasica(string json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGPViabilidadGuardarInformacionBasica"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _datosServicios.SGPViabilidadGuardarInformacionBasica(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Viabilidad/FirmaUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> SGPViabilidadFirmarUsuario(string json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGPViabilidadFirmarUsuario"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _datosServicios.SGPViabilidadFirmarUsuario(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/LeerProyectoViabilidadInvolucrados")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los involucrados del proyecto y tipo concepto viabilidad", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGPProyectosLeerProyectoViabilidadInvolucrados"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPProyectosLeerProyectoViabilidadInvolucrados(proyectoId, instanciaId, tipoConceptoViabilidadId));
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

        [Route("SGP/Proyectos/ObtenerEntidadDestinoResponsableFlujo")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene la entidad destino responsable parametrizada en el flujo para los involucrados del proyecto", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGPProyectosLeerProyectoViabilidadInvolucrados"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPProyectosObtenerEntidadDestinoResponsableFlujo(rolId, crTypeId, entidadResponsableId, proyectoId));
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

        [Route("SGP/Proyectos/ObtenerEntidadDestinoResponsableFlujoTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene la entidad destino responsable parametrizada en el flujo para los involucrados en el tramite", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGPProyectosLeerProyectoViabilidadInvolucrados"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(rolId, entidadResponsableId, tramiteId));
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

        [Route("SGP/Proyectos/GuardarProyectoViabilidadInvolucradosSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar involucrado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoViabilidadInvolucradosSGP(ProyectoViabilidadInvolucradosDto proyectoViabilidadInvolucradosDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GuardarProyectoViabilidadInvolucradosSGP"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = new ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto>();
                parametrosGuardar.Contenido = proyectoViabilidadInvolucradosDto;

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _datosServicios.GuardarProyectoViabilidadInvolucradosSGP(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("SGP/Proyectos/EliminarProyectoViabilidadInvolucradosSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar involucrado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoViabilidadInvolucradosSGP(int id)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["EliminarProyectoViabilidadInvolucradosSGP"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);
                await Task.Run(() => _datosServicios.EliminarProyectoViabilidadInvolucradosSGP(id));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };
                return Ok(respuesta);

            }
            catch (ServiciosNegocioException e)
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("SGP/Proyectos/LeerProyectoViabilidadInvolucradosFirma")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los involucrados del proyecto y tipo concepto viabilidad para firma", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGPProyectosLeerProyectoViabilidadInvolucradosFirm"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPProyectosLeerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId));
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