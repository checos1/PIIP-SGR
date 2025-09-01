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
    using DNP.ServiciosNegocio.Comunes.Dto;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
    using System.Net.Http;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
    using System;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
    using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.OcadPaz;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTEI;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.AvalUso;
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
    using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;

    [Route("api/[controller]")]
    public class ProyectoSgrController : ApiController
    {
        private readonly IProyectoSgrServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ProyectoSgrController(IProyectoSgrServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/Proyectos/LeerListas")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee una lista para un proyecto según parámetro", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerListas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerListas(nivelId, proyectoId, nombreLista));
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

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="delegado"></param> 
        /// <param name="user"></param> 
        /// <returns>Json</returns> 
        [Route("SGR/Proyectos/ActualizarEntidadAdscrita")]
        [SwaggerResponse(HttpStatusCode.OK, "Actualizar Entidad Adscrita", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, int entityId, bool delegado, string user)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                        RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                        ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                        ConfigurationManager.AppSettings["SGR_Proyectos_ActualizarEntidadAdscrita"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_ActualizarEntidadAdscrita(proyectoId, entityId, delegado, user));

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

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>Json</returns> 
        [Route("SGR/Proyectos/LeerEntidadesAdscritas")]
        [SwaggerResponse(HttpStatusCode.OK, "Leer entidades por id del proyecto", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerEntidadesAdscritas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, tipoEntidad));
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

        /// <summary>
        /// Validar cumplimiento de un proyecto por instancia
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <returns>int</returns> 
        [Route("SGR/Proyectos/CumplimentoFlujoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Validar cumplimiento de un proyecto por instancia", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_CumplimentoFlujoSGR(Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_CumplimentoFlujoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_CumplimentoFlujoSGR(instanciaId));
                
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

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipo"></param> 
        /// <returns>Json</returns> 
        [Route("SGR/Proyectos/ValidarEntidadDelegada")]
        [SwaggerResponse(HttpStatusCode.OK, "valida entidad delegada por id del proyecto", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerEntidadesAdscritas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo));
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

        [Route("SGR/Proyectos/LeerProyectoViabilidadInvolucrados")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los involucrados del proyecto y tipo concepto viabilidad", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerProyectoViabilidadInvolucrados"]).Result;

                //if (!respuestaAutorizacion.IsSuccessStatusCode)
                //    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId));
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

        [Route("SGR/Proyectos/ProyectoViabilidadInvolucrados/Agregar")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar involucrado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoViabilidadInvolucrados(ProyectoViabilidadInvolucradosDto proyectoViabilidadInvolucradosDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["SGR_Proyectos_LeerProyectoViabilidadInvolucrados"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = new ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto>();
                parametrosGuardar.Contenido = proyectoViabilidadInvolucradosDto;

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _datosServicios.GuardarProyectoViabilidadInvolucrados(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("SGR/Proyectos/ProyectoViabilidadInvolucrados/Eliminar")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar involucrado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoViabilidadInvolucradoso(int id)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["SGR_Proyectos_LeerProyectoViabilidadInvolucrados"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _datosServicios.EliminarProyectoViabilidadInvolucradoso(id));
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

        [Route("SGR/Proyectos/LeerProyectoViabilidadInvolucradosFirma")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los involucrados del proyecto y tipo concepto viabilidad para firma", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerProyViabilidadInvolucradosFirma"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId));
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

        [Route("SGR/Proyectos/GenerarMensajeEstadoProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene el mensaje del estado de proyecto al finalizar la viabilidad", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_GenerarMensajeEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_GenerarMensajeEstadoProyecto(instanciaId));
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

        [Route("SGR/Proyectos/PostAplicarFlujoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Aplica acciones posteriores a la finalización del flujo SGR", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_PostAplicarFlujoSGR(AplicarFlujoSGRDto parametros)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_GenerarMensajeEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_PostAplicarFlujoSGR(parametros));
                if (result) return Ok(result);

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


        [Route("SGR/Proyectos/PostDevolverFlujoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Aplica acciones posteriores a la devolución del flujo SGR", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_PostDevolverFlujoSGR(DevolverFlujoSGRDto parametros)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_GenerarMensajeEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_PostDevolverFlujoSGR(parametros));
                if (result) return Ok(result);

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

        [Route("SGR/Proyectos/LeerProyectoCtus")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee la información del proyecto para asignar CTUS", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerProyectoCtus"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerProyectoCtus(ProyectoId, instanciaId));
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

        [Route("SGR/Proyectos/LeerEntidadesSolicitarCtus")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee la información del proyecto para asignar CTUS", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerEntidadesSolicitarCtus"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerEntidadesSolicitarCtus(ProyectoId));
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

        [Route("SGR/Proyectos/GuardarProyectoCtus")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar PROYECTOctus", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoViabilidadInvolucrados(ProyectoCtusDto proyectoCtusDto)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["SGR_Proyectos_GuardarProyectoCtus"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = new ParametrosGuardarDto<ProyectoCtusDto>();
                parametrosGuardar.Contenido = proyectoCtusDto;

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _datosServicios.SGR_Proyectos_GuardarProyectoSolicitarCtus(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGR/ConsultarProyectosVerificacionOcadPazSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectoEntidadDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosVerificacionOcadPazSgr(ParametrosProyectoVerificacionSgrDto parametros)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var response = await Task.Run(() => _datosServicios.ConsultarProyectosVerificacionOcadPazSgr(parametros));

                return Ok(response);
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

        [Route("SGR/OCADPaz/ObtenerUsuariosVerificacionOcadPaz")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna usuarios a partir de rol y entidad para verificación OCAD Paz", typeof(UsuariosVerificacionOcadPazDto))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(Guid rolId, int entidadId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var response = await Task.Run(() => _datosServicios.SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(rolId, entidadId));

                return Ok(response);
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

        [Route("SGR/OCADPaz/GuardarAsignacionUsuarioEncargado")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_OCADPaz_GuardarAsignacionUsuarioEncargado"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Proyectos/validarTecnicoOcadpaz")]
        [SwaggerResponse(HttpStatusCode.OK, "Validación inicial en el paso de verificación técnico de ocad paz.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_validarTecnicoOcadpaz(Nullable<System.Guid> instanciaId, Nullable<System.Guid> accionId)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_validarTecnicoOcadpaz"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_validarTecnicoOcadpaz(instanciaId, accionId));
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

        [Route("SGR/Proyectos/GuardarAsignacionUsuarioEncargado")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_Proyectos_GuardarAsignacionUsuarioEncargado"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_GuardarAsignacionUsuarioEncargado(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Proyectos/LeerAsignacionUsuarioEncargado")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los usuarios técnicos asignados para concepto ", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerAsignacionUsuarioEncargado(int ProyectoId, Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_LeerAsignacionUsuarioEncargado"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerAsignacionUsuarioEncargado(ProyectoId, instanciaId));
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

        [Route("SGR/Proyectos/LeerDatosAdicionalesCTEI")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los datos adicionales de proyectos CTEI", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_CTEI_LeerDatosAdicionales"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerDatosAdicionalesCTEI(proyectoId, instanciaId));
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

        [Route("SGR/Proyectos/GuardarDatosAdicionalesCTEI")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto datosAdicionalesCTEIDto)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_CTEI_GuardarDatosAdicionales"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_GuardarDatosAdicionalesCTEI(datosAdicionalesCTEIDto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Proyectos/RegistrarAvalUsoSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_RegistrarAvalUsoSgr(DatosAvalUsoDto datosAvalUsoDto)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_AvalUso_RegistrarAvalUso"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_RegistrarAvalUsoSgr(datosAvalUsoDto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Proyectos/LeerAvalUsoSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee los datos almacenados de aval de uso", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerAvalUsoSgr(int proyectoId, Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_AvalUso_LeerAvalUso"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Proyectos_LeerAvalUsoSgr(proyectoId, instanciaId));
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

        [Route("SGR/Proyectos/TieneInstanciaActiva")]
        [SwaggerResponse(HttpStatusCode.OK, "Muestra si un proyecto tiene una instancia activa", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Obtener_Proyectos_TieneInstanciaActiva(String ObjetoNegocioId)
        {
            try
            {
                var result = await Task.Run(() => _datosServicios.SGR_Obtener_Proyectos_TieneInstanciaActiva(ObjetoNegocioId));
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

        [Route("SGR/Proyectos/EliminarOperacionCreditoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_EliminarOperacionCreditoSGR(int proyectoid)
        {
            try
            {
                var result = await Task.Run(() => _datosServicios.SGR_Viabilidad_EliminarOperacionCreditoSGR(proyectoid, RequestContext.Principal.Identity.Name));
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
    }
}