using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.DesignacionEjecutor;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.GestionRecursos.Aprobacion;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Procesos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.GestionRecursos
{
    public class ProcesosSgrController : ApiController
    {

        private readonly IProcesosSgrServicio _procesosSgrServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ProcesosSgrController(IProcesosSgrServicio procesosSgrServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _procesosSgrServicio = procesosSgrServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/Procesos/ObtenerPriorizacionProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener priorizacion proyecto", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_PriorizacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerPriorizacionProyecto(instanciaId));
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

        [Route("SGR/Procesos/ObtenerAprobacionProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener aprobación proyecto", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_AprobacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerAprobacionProyecto(instanciaId));
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

        [Route("SGR/Procesos/ObtenerProyectoAprobacionInstanciasSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Aprobacion Proyecto Detalle SGR", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoAprobacionInstanciasSGR(Nullable<Guid> instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_AprobacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerProyectoAprobacionInstanciasSGR(instanciaId));
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

        [Route("SGR/Procesos/GuardarProyectoAprobacionInstanciasSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Aprobacion Proyecto Detalle SGR", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_AprobacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                await Task.Run(() => _procesosSgrServicio.GuardarProyectoAprobacionInstanciasSGR(proyectoAprobacionInstanciasDto, RequestContext.Principal.Identity.Name));

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

        [Route("SGR/Procesos/ObtenerAprobacionProyectoCredito")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener datos de Aprobacion Proyecto Credito", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAprobacionCredito(Guid instancia, int entidad)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_AprobacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerAprobacionProyectoCredito(instancia, entidad));
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

        [Route("SGR/Procesos/GuardarAprobacionProyectoCredito")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar datos de Aprobacion Proyecto Credito", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarAprobacionCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_AprobacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.GuardarAprobacionProyectoCredito(aprobacionProyectoCreditoDto, RequestContext.Principal.Identity.Name));

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuestaHttp);
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

        [Route("SGR/Procesos/GuardarProyectoPermisosProcesoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Permisos para los procesos de priorizacion y aprobacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto)
        {
            try
            {
                await Task.Run(() => _procesosSgrServicio.GuardarProyectoPermisosProcesoSGR(proyectoProcesoDto, RequestContext.Principal.Identity.Name));

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

        [Route("SGR/Procesos/ObtenerPriorizionProyectoDetalleSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Priorizion Proyecto Detalle SGR", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_PriorizacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerPriorizionProyectoDetalleSGR(instanciaId));
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

        [Route("SGR/Procesos/GuardarPriorizionProyectoDetalleSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Priorizion Proyecto Detalle SGR", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_PriorizacionProyectoSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                await Task.Run(() => _procesosSgrServicio.GuardarPriorizionProyectoDetalleSGR(proyectoPriorizacionDetalleDto, RequestContext.Principal.Identity.Name));

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

        [Route("SGR/Procesos/MostrarEstadosPriorizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Muestra el estado de la priorización de un proyecto y sus metodologías", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Proyectos_MostrarEstadosPriorizacion"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.SGR_Proyectos_MostrarEstadosPriorizacion(proyectoId));
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

        [Route("SGR/Procesos/ObtenerProyectoResumenAprobacionSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Muestra el estado de la aprobación de un proyecto y sus metodologías", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoResumenAprobacionSGR(int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["SGR_Proyectos_MostrarEstadosAprobacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _procesosSgrServicio.SGR_Proyectos_MostrarEstadosAprobacion(proyectoId));
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

        [Route("SGR/Procesos/LeerValoresAprobacionSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Muestra los valores de la aprobación de un proyecto y la descripción de las etapas asociadas", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> LeerValoresAprobacionSGR(int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ProcesoDesignacionEjecutorSGR"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _procesosSgrServicio.SGR_Proyectos_LeerValoresAprobacion(proyectoId));
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

        [Route("SGR/Procesos/ObtenerProyectoResumenAprobacionCreditoParcialSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Muestra el estado de las instancias que forman parte de un esquema de aprobación OPC, pero que actualmente no tienen un crédito asociado.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoResumenAprobacionCreditoParcial(int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["SGR_Proyectos_MostrarEstadosAprobacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _procesosSgrServicio.SGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(proyectoId));
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

        [Route("SGR/Procesos/ConsultarEjecutorbyTipo")]
        [SwaggerResponse(HttpStatusCode.OK, "consultar Ejecutor Interventores Supervisores Asociados a un Proyecto", typeof(EjecutorEntidadAsociado))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecutorId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["SGR_Procesos_ConsultarEjecutorbyTipo"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _procesosSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(proyectoId, tipoEjecutorId));
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

        [Route("SGR/Procesos/ObtenerProyectoResumenEstadoAprobacionCreditoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Muestra el estado de la aprobación de un proyecto y sus metodologías", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoResumenEstadoAprobacionCreditoSGR(int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["SGR_Proyectos_MostrarEstadosAprobacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _procesosSgrServicio.SGR_Proyectos_ResumenEstadoAprobacionCredito(proyectoId));
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

        #region Designación Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param>    
        /// <returns>bool</returns> 
        [Route("SGR/Procesos/RegistrarRespuestaEjecutorSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar valor de una columna dinamica del ejecutor por proyectoId.", typeof(bool))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["ProcesoDesignacionEjecutorSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.RegistrarRespuestaEjecutorSGR(valores, RequestContext.Principal.Identity.Name));

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
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <returns>string</returns>
        [Route("SGR/Procesos/ObtenerRespuestaEjecutorSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener el valor de una columna dinámica del ejecutor por proyectoId.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerRespuestaEjecutorSGR(string campo, int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["ProcesoDesignacionEjecutorSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerRespuestaEjecutorSGR(campo, proyectoId));
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
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>     
        /// <param name="valores"></param>       
        /// <returns>bool</returns>
        [Route("SGR/Procesos/ActualizarValorEjecutorSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar valor de dinamico en la tabla de aprobación valores.", typeof(bool))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarValorEjecutorSGR(CampoItemValorDto valores)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["ProcesoDesignacionEjecutorSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ActualizarValorEjecutorSGR(valores, RequestContext.Principal.Identity.Name));

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
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>
        /// <param name="instanciaId"></param>     
        /// <returns>string</returns>
        [Route("SGR/Procesos/ObtenerValorCostosEstructuracionViabilidadSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener valor de costos de estructuracion viabilidad.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["ProcesoDesignacionEjecutorSGR"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _procesosSgrServicio.ObtenerValorCostosEstructuracionViabilidadSGR(instanciaId));
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

        #endregion Designación Ejecutor
    }
}
