
namespace DNP.ServiciosNegocio.Web.API.Controllers.TramitesProyectos
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Dto.ObjetosNegocio;
    using Comunes.Excepciones;
    using Dominio.Dto.Proyectos;
    using Servicios.Interfaces.Proyectos;
    using Swashbuckle.Swagger.Annotations;
    using System;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using Newtonsoft.Json.Linq;
    using Comunes.Dto;
    using Servicios.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using System.Drawing;
    using System.IO;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;

    public class TramitesProyectosController : ApiController
    {
        private readonly ITramitesProyectosServicio _tramitesProyectosServicio;
        private readonly ICambiosRelacionPlanificacionServicio _relacionPlanificacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly ISeccionCapituloServicio _seccionCapituloServicio;
        private readonly ICambiosJustificacionHorizonServicio _justificacionHorizonteServicio;

        public TramitesProyectosController(ITramitesProyectosServicio tramitesProyectosServicio,
            IAutorizacionUtilidades autorizacionUtilidades,
            ICambiosRelacionPlanificacionServicio relacionPlanificacionServicio,
            ISeccionCapituloServicio seccionCapituloServicio,
            ICambiosJustificacionHorizonServicio justificacionHorizonteServicio)
        {
            _tramitesProyectosServicio = tramitesProyectosServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
            _relacionPlanificacionServicio = relacionPlanificacionServicio;
            _seccionCapituloServicio = seccionCapituloServicio;
            _justificacionHorizonteServicio = justificacionHorizonteServicio;
        }

        [Route("api/TramitesProyectos/GuardarProyectosTramiteNegocio")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectosTramiteNegocio([FromBody] DatosTramiteProyectosDto DatosTramiteProyectosDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _tramitesProyectosServicio.ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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


        [Route("api/TramitesProyectos/GuardarTramiteTipoRequisito")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteTipoRequisito([FromBody] List<TramiteRequitoDto> datosProyectoRequisitoDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = datosProyectoRequisitoDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _tramitesProyectosServicio.GuardarTramiteTipoRequisito(parametrosGuardar, RequestContext.Principal.Identity.Name));

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


        [Route("api/TramitesProyectos/GuardarTramiteInformacionPresupuestal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteInformacionPresupuestal([FromBody] List<TramiteFuentePresupuestalDto> datosTramitesInformacionPresupuestalDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = datosTramitesInformacionPresupuestalDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _tramitesProyectosServicio.GuardarTramiteInformacionPresupuestal(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/TramitesProyectos/ObtenerProyectosTramiteNegocio")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectosEnTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosTramiteNegocio([FromUri] int TramiteId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectosTramiteNegocio(TramiteId));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/ObtenerTipoDocumentoTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna tipo documento por tipo tramite", typeof(TipoDocumentoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTipoDocumentoTramite([FromUri] int TipoTramiteId, Guid? Rol, int tramiteId, int nivelId)
        {
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerTipoDocumentoTramite(TipoTramiteId, Rol, tramiteId, nivelId));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/EliminarProyectosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                       RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                       ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                       ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var result = await Task.Run(() => _tramitesProyectosServicio.EliminarProyectoTramiteNegocio(TramiteId, ProyectoId));


                return Ok(result);
            }
            catch (Exception exception)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = exception.Message
                });
            }
        }

        [Route("api/TramitesProyectos/ObtenerFuentesInformacionPresupuestal")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesInformacionPresupuestal()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                  RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                  ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                  ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerFuentesInformacionPresupuestal());

            return Ok(result);
        }

        [Route("api/TramitesProyectos/ObtenerProyectoFuentePresupuestalPorTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                 RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                 ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                 ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectoFuentePresupuestalPorTramite(pProyectoId, pTramiteId, pTipoProyecto));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/ObtenerProyectoRequisitosPorTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, isCDP));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/ActualizarInstanciaProyectosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarInstanciaProyectosTramiteNegocio([FromBody] Dominio.Dto.Proyectos.ProyectoTramiteDto DatosInstanciasProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                await Task.Run(() => _tramitesProyectosServicio.ActualizarInstanciaProyectosTramiteNegocio(DatosInstanciasProyecto, RequestContext.Principal.Identity.Name));

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

        [Route("api/TramitesProyectos/ObtenerPreguntasJustificacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasJustificacion([FromUri] int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var TokenAutorizacion = Request.Headers.Authorization.ToString();
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPreguntasJustificacion(TramiteId, ProyectoId, TipoTramiteId, TipoRolId, IdNivel));

                return Ok(result);
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        [Route("api/TramitesProyectos/GuardarRespuestasJustificacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> justificacionTramiteProyectoDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarRespuestasJustificacion(justificacionTramiteProyectoDto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/TramitesProyectos/ActualizarValoresProyectosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarValoresProyectosTramiteNegocio([FromBody] Dominio.Dto.Proyectos.ProyectoTramiteDto DatosInstanciasProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizarValoresProyectosTramiteNegocio(DatosInstanciasProyecto, RequestContext.Principal.Identity.Name));

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

        [Route("api/TramitesProyectos/ObtenerTiposRequisito")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTiposRequisito()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerTiposRequisito());

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


        [Route("api/TramitesProyectos/ValidarEnviarDatosTramiteNegocio")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarEnviarDatosTramiteNegocio([FromBody] DatosTramiteProyectosDto DatosTramiteProyectosDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _tramitesProyectosServicio.ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramitesProyectosServicio.ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/TramitesProyectos/ObtenerPreguntasProyectoActualizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasProyectoActualizacion([FromUri] int TramiteId, int ProyectoId, int TipoTramiteId,Guid IdNivel, int TipoRolId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPreguntasProyectoActualizacion(TramiteId, ProyectoId, TipoTramiteId, IdNivel,TipoRolId));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/ObtenerProyectosTramiteNegocioAprobacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectosEnTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosTramiteNegocioAprobacion([FromUri] int TramiteId, int TipoRolId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectosTramiteNegocioAprobacion(TramiteId, TipoRolId));

            return Ok(result);
        }


        [Route("api/TramitesProyectos/ObtenerFuentesTramiteProyectoAprobacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(FuentesTramiteProyectoAprobacionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesTramiteProyectoAprobacion([FromUri] int tramiteId, int proyectoId, string pTipoProyecto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerFuentesTramiteProyectoAprobacion(tramiteId, proyectoId, pTipoProyecto));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/GuardarFuentesTramiteProyectoAprobacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarFuentesTramiteProyectoAprobacion(fuentesTramiteProyectoAprobacion, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerCodigoPresupuestal")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerCodigoPresupuestal(proyectoId, entidadId, tramiteId, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ActualizarCodigoPresupuestal")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizarCodigoPresupuestal(proyectoId, entidadId, tramiteId, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/CrearAlcanceTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearAlcanceTramite(AlcanceTramiteDto data)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.CrearAlcanceTramite(data));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/GuardarSolicitarConcepto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarSolicitarConcepto([FromBody] EnvioSubDireccionDto concepto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarSolicitarConcepto(concepto));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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
        [Route("api/TramitesProyectos/ObtenerSolicitarConcepto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerSolicitarConcepto([FromBody] EnvioSubDireccionDto concepto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerSolicitarConcepto(concepto.TramiteId));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/TramitesProyectos/ObtenerTarmitesPorProyectoEntidad")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuarioDnp)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerTarmitesPorProyectoEntidad(proyectoId, entidadId, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerValoresProyectos")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerValoresProyectos(proyectoId, tramiteId, entidadId,  RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerConceptoDireccionTecnicaTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerConceptoDireccionTecnicaTramite(tramiteId, nivelid, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/GuardarConceptoDireccionTecnicaTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramite> lConceptoDireccionTecnicaTramite)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarConceptoDireccionTecnicaTramite(lConceptoDireccionTecnicaTramite, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ValidarEnviarDatosTramiteNegocioAprobacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarEnviarDatosTramiteNegocioAprobacion([FromBody] DatosTramiteProyectosDto DatosTramiteProyectosDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _tramitesProyectosServicio.ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramitesProyectosServicio.ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/TramitesProyectos/ObtenerPlantillaCarta")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPlantillaCarta(nombreSeccion, tipoTramite));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerDatosCartaPorSeccion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult>  ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosCartaPorSeccion(tramiteId, plantillaSeccionId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerDatosCartaPorSeccionDespedida")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCartaPorSeccionDespedida(int plantillaSeccionId, int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosCartaPorSeccionDespedia(plantillaSeccionId, tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/VerificaUsuarioDestinatario")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> VerificaUsuarioDestinatario([FromBody] UsuarioTramite usuarioTramite)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramitesProyectosServicio.VerificaUsuarioDestinatario(usuarioTramite));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/TramitesProyectos/ActualizarCartaDatosIniciales")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCartaDatosIniciales([FromBody] Carta datosIniciales)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizarCartaDatosIniciales(datosIniciales, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/TramitesProyectos/ActualizarCartaDatosDespedida")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCartaDatosDespedida([FromBody] Carta datosDespedida)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizarCartaDatosDespedida(datosDespedida, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
                //return Ok(new RespuestaGeneralDto { Exito = true });
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

        [Route("api/TramitesProyectos/ObtenerUsuariosRegistrados")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerUsuariosRegistrados(tramiteId, numeroTramite));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/CargarFirma")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> CargarFirma(FileToUploadDto parametros)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.CargarFirma(parametros));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ValidarSiExisteFirmaUsuario")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ValidarSiExisteFirmaUsuario( string idUsuario)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ValidarSiExisteFirmaUsuario(idUsuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/Firmar")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> Firmar(int tramiteId, string radicadoSalida)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.Firmar(tramiteId, radicadoSalida, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerCuerpoConceptoCDP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCartaConceptoCuerpoCDP(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosCartaConceptoCuerpoCDP(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerCuerpoConceptoAutorizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCartaConceptoCuerpoAutorizacion(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosCartaConceptoCuerpoAutorizacion(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerCambiosFirmeRelacionPlanificacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCambiosFirmeRelacionPlanificacion(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _relacionPlanificacionServicio.ConsultarCambiosConpes(proyectoId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/guardarCambiosFirmeRelacionPlanificacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCambiosFirmeRelacionPlanificacion([FromBody] CapituloModificado capituloModificado)
        {
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var resultActualizacion = await Task.Run(() => _relacionPlanificacionServicio.GuardarJustificacionCambios(capituloModificado, usuario));
                var result = new TramitesResultado() {
                    Exito = resultActualizacion,
                    Mensaje = resultActualizacion ? "Los datos fueron guardados con éxito" : "No fue posible actualizar la información"
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/TramitesProyectos/ObtenerJustificacionHorizonte")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerJustificacionHorizonte(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _justificacionHorizonteServicio.ObtenerCambiosJustificacionHorizonte(proyectoId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/guardarCambiosFirmeJustificacionHorizonte")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> guardarCambiosFirmeJustificacionHorizonte([FromBody] CapituloModificado capituloModificado)
        {
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var resultActualizacion = await Task.Run(() => _justificacionHorizonteServicio.GuardarJustificacionCambios(capituloModificado, usuario));
                var result = new TramitesResultado()
                {
                    Exito = resultActualizacion,
                    Mensaje = resultActualizacion ? "Los datos fueron guardados con éxito" : "No fue posible actualizar la información"
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/ActualizarEstadoAjusteProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizaEstadoAjusteProyecto([FromUri] string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["ActualizaEstadoAjusteProyecto"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizaEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ConsultarCarta")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCarta(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ConsultarCarta(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerConpesTramite/{tramiteId}")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        public async Task<IHttpActionResult> ObtenerConpesTramite(int tramiteId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = _tramitesProyectosServicio.ObtenerConpesTramite(tramiteId);
            if(result.Estado) {
                return Ok(result);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.Mensaje));
        }

        [Route("api/Tramites/AsociarTramiteConpes")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociarTramiterConpes([FromBody] AsociarTramiteConpesRequestDto model)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = _tramitesProyectosServicio.GuardarConpesTramite(model, RequestContext.Principal.Identity.Name);
            if (result.Estado)
            {
                return Ok(result);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.Mensaje));
        }


        [Route("api/Tramites/RemoverAsociacionTramiteConpes")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoverAsociacionTramiteConpes([FromBody] RemoverAsociacionConpesTramiteDto model)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = _tramitesProyectosServicio.RemoverConpesTramite(model);
            if (result.Estado) {
                return Ok(result);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.Mensaje));
        }

        [Route("api/Tramites/ObtenerPeriodoPresidencial")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPeriodoPresidencial()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = _tramitesProyectosServicio.ObtenerPeriodoPresidencialActual();
            if (result.Estado) {
                return Ok(result);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.Mensaje));
        }

        /// <summary>
        /// Api para eliminar asociación de proyecto a tramite Vigencias Futuras.
        /// </summary>
        /// <param name="eliminacionAsociacionDto">Contiene filtro para la eliminación</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/EliminarAsociacionVFO")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarAsociacionVFO([FromBody] EliminacionAsociacionDto eliminacionAsociacionDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.EliminarAsociacionVFO(eliminacionAsociacionDto));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar los proyectos que se pueden asociar a tramite Vigencias Futuras.
        /// </summary>
        /// <param name="ObtenerProyectoAsociacionVFO">Contiene filtro para proyectos a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/ObtenerProyectoAsociacionVFO")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoAsociacionVFO(string Bpin, int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectoAsociarTramite(Bpin, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api asociar proyecto a tramite Vigencias Futuras.
        /// </summary>
        /// <param name="AsociarProyectoVFO">Contiene datos del proyecto a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/AsociarProyectoVFO")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociarProyectoVFO([FromBody] proyectoAsociarTramite proyectoAsociarTramiteDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.AsociarProyectoVFO(proyectoAsociarTramiteDto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        

        [Route("api/Tramites/ObtenerDatosProyectoTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna datos del proyecto por tramite", typeof(DatosProyectoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProyectoTramite(int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["ObtenerDatosProyectoTramite"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosProyectoTramite(TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerDetalleCartaTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna datos del proyecto por tramite", typeof(DatosProyectoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleCartaTramite(int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["ObtenerDatosProyectoTramite"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDetalleCartaConcepto(TramiteId));
                if(result != null) {
                    return Ok(result);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No fue posible obtener la carta concepto seleccionada" ));

            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ActualizarVigenciaFuturaFuente")]
        [SwaggerResponse(HttpStatusCode.OK, "ActualizarVigenciaFuturaFuente", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["ObtenerDatosProyectoTramite"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _tramitesProyectosServicio.ActualizarVigenciaFuturaFuente(fuente, RequestContext.Principal.Identity.Name));

                return Ok(resultado);
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

        [Route("api/Tramites/ActualizarVigenciaFuturaProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "ActualizarVigenciaFuturaProducto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod)
        {
            try
            {
                var resultado = await Task.Run(() => _tramitesProyectosServicio.ActualizarVigenciaFuturaProducto(prod, RequestContext.Principal.Identity.Name));

                return Ok(resultado);
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

        [Route("api/Tramites/ObtenerDatosProyectosPorTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna datos del proyecto por tramite", typeof(DatosProyectoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProyectosPorTramite(int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["ObtenerDatosProyectoTramite"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosProyectosPorTramite(TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #region Vigencias Futuras

        /// <summary>
        /// Api para consultar los valores constantes de información presupuestal en Vigencias Futuras.
        /// </summary>
        /// <param name="ObtenerInformacionPresupuestalVlrConstanteVF">Contiene filtro para proyectos a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/ObtenerInformacionPresupuestalVlrConstanteVF")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Presupuestal Vlr Constante VF", typeof(InformacionPresupuestalVlrConstanteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalVlrConstanteVF(int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["obtenerInformacionPresupuestalVlrConstanteVF"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerInformacionPresupuestalVlrConstanteVF(TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerSolicitarConceptoPorTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna datos del proyecto por tramite", typeof(DatosProyectoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult>  ObtenerSolicitarConceptoPorTramite(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerSolicitarConceptoPorTramite(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerDatosCronograma")]
        [SwaggerResponse(HttpStatusCode.OK, "Cronograma precontractual y contractual", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCronograma(Guid instanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ObtenerDatosCronograma"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosCronograma(instanciaId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerPreguntasProyectoActualizacionPaso")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasProyectoActualizacionPaso([FromUri] int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["ObtenerPreguntasProyectoActualizacionPaso"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

           // var TokenAutorizacion = Request.Headers.Authorization.ToString();
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId));

            return Ok(result);
        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerDeflactores")]
        [SwaggerResponse(HttpStatusCode.OK, "Deflactores", typeof(List<TramiteDeflactoresDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDeflactores()
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.GetTramiteDeflactores());
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerProyectoTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerProyectoTramite", typeof(List<Dominio.Dto.Tramites.TramiteProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoTramite(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.GetProyectoTramite(ProyectoId, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="tramiteProyectoDto">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ActualizaVigenciaFuturaProyectoTramite")]
        //[SwaggerResponse(HttpStatusCode.OK, "ActualizaVigenciaFuturaProyectoTramite", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizaVigenciaFuturaProyectoTramite([FromBody] Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerFuentesFinanciacionVigenciaFuturaCorriente")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerFuentesFinanciacionVigenciaFuturaCorriente", typeof(VigenciaFuturaCorrienteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.GetFuentesFinanciacionVigenciaFuturaCorriente(bpin));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerFuentesFinanciacionVigenciaFuturaConstante")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerFuentesFinanciacionVigenciaFuturaConstante", typeof(VigenciaFuturaConstanteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.GetFuentesFinanciacionVigenciaFuturaCoonstante(bpin, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar los valores constantes/Corrientes de información presupuestal en Vigencias Futuras.
        /// </summary>
        /// <param name="ObtenerInformacionPresupuestalValores">Contiene filtro para proyectos a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/ObtenerInformacionPresupuestalValores")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Presupuestal Valores", typeof(InformacionPresupuestalValoresDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalValores(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["obtenerInformacionPresupuestalVlrConstanteVF"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerInformacionPresupuestalValores(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Guardado de valores aprobados en Vigencias Futuras.
        /// </summary>
        /// <param name="informacionPresupuestalValoresDto">Instancia</param>
        /// <returns></returns>
        [Route("api/Tramites/GuardarInformacionPresupuestalValores")]
        //[SwaggerResponse(HttpStatusCode.OK, "GuardarInformacionPresupuestalValores", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarInformacionPresupuestalValores([FromBody] InformacionPresupuestalValoresDto informacionPresupuestalValoresDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                  RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                  ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                  ConfigurationManager.AppSettings["obtenerInformacionPresupuestalVlrConstanteVF"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarInformacionPresupuestalValores(informacionPresupuestalValoresDto, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion Vigencias Futuras

        /// <summary>
        /// Api para consultar la información de la accion de la instancia.
        /// </summary>
        /// <param name="tramiteId">tramiteId</param>
        /// <returns>Datos de la accion</returns>
        [Route("api/Tramites/ObtenerAccionActualyFinal")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerAccionActualyFinal", typeof(AccionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAccionActualyFinal(int tramiteId, string bpin)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerAccionActualyFinal(tramiteId, bpin));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para eliminar permiso accion  usuario.
        /// </summary>
        /// <param name="usuarioDestino">usuarioDestino</param>
        /// <returns>si elimino o hay error</returns>
        [Route("api/Tramites/EliminarPermisosAccionesUsuarios")]
        [SwaggerResponse(HttpStatusCode.OK, "EliminarPermisosAccionesUsuarios", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid))
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.EliminarPermisosAccionesUsuarios(usuarioDestino, tramiteId, aliasNivel, InstanciaId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Enivar solicitud concepto.
        /// </summary>
        /// <param name="tramiteId">usuarioDestino</param>
        /// <returns>si envió o hay error</returns>
        [Route("api/Tramites/EnviarConceptoDireccionTecnicaTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "EnviarConceptoDireccionTecnicaTramite", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.EnviarConceptoDireccionTecnicaTramite(tramiteId, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerModalidadesContratacion")]
        [SwaggerResponse(HttpStatusCode.OK, "ModalidadesContratacion", typeof(List<TramiteModalidadContratacionDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerModalidadesContratacion(int? mostrar)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerModalidadesContratacion(mostrar));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ActualizarActividadesCronograma")]
        [SwaggerResponse(HttpStatusCode.OK, "ActividadesPrecontractualesModalidadesContratacion", typeof(ActividadPreContractualDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarActividadesCronograma(ActividadPreContractualDto actividades)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizarActividadesCronograma(actividades, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// Api para consultar la información del cronograma precontractual y contractual en Vigencias Futuras.
        /// </summary>
        /// <param name="instanciaId">Instancia</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerActividadesPrecontractualesProyectoTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "ActividadesPrecontractualesProyectoTramite", typeof(ActividadPreContractualDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerActividadesPrecontractualesProyectoTramite(ModalidadContratacionId, ProyectoId, TramiteId, eliminarActividades));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información de ObtenerProductosVigenciaFuturaConstante.
        /// </summary>
        /// <param name="bpin">bpin</param>
        /// <param name="TramiteId">TramiteId</param>
        /// <param name="AnioBase">AnioBase</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerProductosVigenciaFuturaConstante")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerProductosVigenciaFuturaConstante", typeof(ProductosConstantesVF))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProductosVigenciaFuturaConstante(string bpin, int TramiteId, int AnioBase)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.GetProductosVigenciaFuturaConstante(bpin, TramiteId, AnioBase));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar la información de ObtenerProductosVigenciaFuturaConstante.
        /// </summary>
        /// <param name="bpin">bpin</param>
        /// <param name="TramiteId">TramiteId</param>
        /// <returns>Datos del cronograma</returns>
        [Route("api/Tramites/ObtenerProductosVigenciaFuturaCorriente")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerProductosVigenciaFuturaCorriente", typeof(ProductosCorrientesVF))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProductosVigenciaFuturaCorriente(string bpin, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.GetProductosVigenciaFuturaCorriente(bpin, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerTipoDocumentoTramitePorNivel")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna tipo documento por tipo tramite", typeof(TipoDocumentoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTipoDocumentoTramitePorNivel([FromUri] int tipoTramiteId, string nivelId, string rolId = null)
        {
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerTipoDocumentoTramitePorNivel(tipoTramiteId, nivelId, rolId ));

            return Ok(result);
        }

        [Route("api/TramitesProyectos/ObtenerDatosUsuario")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna liosta usuarios", typeof(TipoDocumentoTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia)
        {
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia));

            return Ok(result);
        }

        /// <summary>
        /// Api para consultar los proyectos que se pueden asociar a tramite Vigencias Futuras.
        /// </summary>
        /// <param name="ObtenerProyectoAsociacionAclaracionLeyenda">Contiene filtro para proyectos a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/ObtenerProyectoAsociacionAclaracionLeyenda")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoAsociacionAclaracionLeyenda(string Bpin, int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectoAsociarTramiteLeyenda(Bpin, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerModificacionLeyenda")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Datos de modificacion de leyenda", typeof(ModificacionLeyendaDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerModificacionLeyenda(int tramiteId, int ProyectoId)
        {
            var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerModificacionLeyenda(tramiteId, ProyectoId));

            return Ok(result);
        }

        /// Api para insertar o modificar la leyenda del proyecto.
        /// </summary>
        /// <param name="ModificacionLeyendaDto">modificacionLeyendaDto</param>
        /// <returns></returns>
        [Route("api/TramitesProyectos/ActualizaModificacionLeyenda")]
        [SwaggerResponse(HttpStatusCode.OK, "ActualizaModificacionLeyenda", typeof(ModificacionLeyendaDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ActualizaModificacionLeyenda(modificacionLeyendaDto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/TramiteEnPasoUno")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> TramiteEnPasoUno(Guid InstanciaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.TramiteEnPasoUno(InstanciaId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerListaDireccionesDNP")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerListaDireccionesDNP", typeof(Guid))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaDireccionesDNP(Guid idEntidad)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerListaDireccionesDNP(idEntidad));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerListaSubdireccionesPorParentId")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerListaSubdireccionesPorParentId", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaSubdireccionesPorParentId(int idEntidadType)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerListaSubdireccionesPorParentId(idEntidadType));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/BorrarFirma")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> BorrarFirma(FileToUploadDto parametros)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.BorrarFirma(parametros));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerProyectosCartaTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion de proyecto de tramite AL", typeof(ProyectosCartaDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosCartaTramite(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerProyectosCartaTramite(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerDetalleCartaAL")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion detalle de la modificacion de leyenda de tramite AL", typeof(DetalleCartaConceptoALDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleCartaAL(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDetalleCartaAL(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerAmpliarDevolucionTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerAmpliarDevolucionTramite", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerAmpliarDevolucionTramite(ProyectoId, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerDatosProyectoConceptoPorInstancia")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerDatosProyectoConceptoPorInstancia", typeof(Guid))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerDatosProyectoConceptoPorInstancia(instanciaId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/FocalizacionActualizaPoliticasModificadas")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> FocalizacionActualizaPoliticasModificadas([FromBody] JustificacionPoliticaModificada capituloModificado)
        {
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var resultActualizacion = await Task.Run(() => _relacionPlanificacionServicio.FocalizacionActualizaPoliticasModificadas(capituloModificado, usuario));
                var result = new TramitesResultado()
                {
                    Exito = resultActualizacion,
                    Mensaje = resultActualizacion ? "Los datos fueron guardados con éxito" : "No fue posible actualizar la información"
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerLiberacionVigenciasFuturas")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerLiberacionVigenciasFuturas", typeof(List<TramiteLiberacionVfDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerLiberacionVigenciasFuturas(ProyectoId, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/InsertaAutorizacionVigenciasFuturas")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(VigenciaFuturaResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.InsertaAutorizacionVigenciasFuturas(autorizacion, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/InsertaValoresUtilizadosLiberacionVF")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(VigenciaFuturaResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.InsertaValoresUtilizadosLiberacionVF(autorizacion, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerListaProyectosFuentes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(List<ProyectoTramiteFuenteDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentes(int tramiteId)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerListaProyectosFuentes(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerEntidadAsociarProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerEntidadAsociarProyecto", typeof(List<EntidadesAsociarComunDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerEntidadAsociarProyecto(InstanciaId, AccionTramiteProyecto));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ConsultarCartaConcepto")]
        [SwaggerResponse(HttpStatusCode.OK, "ConsultarCartaConcepto", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCartaConcepto(int tramiteid)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ConsultarCartaConcepto(tramiteid));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ValidacionPeriodoPresidencial")]
        [SwaggerResponse(HttpStatusCode.OK, "ValidacionPeriodoPresidencial", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ValidacionPeriodoPresidencial(int tramiteid)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ValidacionPeriodoPresidencial(tramiteid));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/GuardarMontosTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarMontosTramite(List<ProyectosEnTramiteDto> parametrosGuardar)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarMontosTramite(parametrosGuardar, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerTramitesVFparaLiberar")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerTramitesVFparaLiberar", typeof(List<tramiteVFAsociarproyecto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTramitesVFparaLiberar(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerTramitesVFparaLiberar(proyectoId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/TramitesProyectos/GuardarLiberacionVigenciaFutura")]
        [SwaggerResponse(HttpStatusCode.OK, "GuardarLiberacionVigenciaFutura", typeof(int))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto parametrosGuardar)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.GuardarLiberacionVigenciaFutura(parametrosGuardar, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerPreguntasJustificacionPorProyectos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPreguntasJustificacionPorProyectos(TramiteId, TipoTramiteId, TipoRolId, IdNivel));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerResumenLiberacionVigenciasFuturas")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerResumenLiberacionVigenciasFuturas", typeof(List<ResumenLiberacionVfDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerResumenLiberacionVigenciasFuturas(ProyectoId, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerValUtilizadosLiberacionVigenciasFuturas")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerValUtilizadosLiberacionVigenciasFuturas", typeof(ValoresUtilizadosLiberacionVfDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerValUtilizadosLiberacionVigenciasFuturas(ProyectoId, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/TramiteAjusteEnPasoUno")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.TramiteAjusteEnPasoUno(tramiteId, proyectoId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerListaProyectosFuentesAprobado")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerListaProyectosFuentesAprobado(tramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/InsertaValoresproductosLiberacionVFCorrientes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(VigenciaFuturaResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.InsertaValoresproductosLiberacionVFCorrientes(productosCorrientes, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/InsertaValoresproductosLiberacionVFConstantes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(VigenciaFuturaResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.InsertaValoresproductosLiberacionVFConstantes(productosConstantes, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerEntidadTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(List<EntidadesAsociarComunDto>))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerEntidadTramite(string numeroTramite)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerEntidadTramite(numeroTramite));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/EliminaLiberacionVF")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Vigencia futura en proceso de liberacion", typeof(VigenciaFuturaResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminaLiberacionVF(LiberacionVigenciasFuturasDto tramiteEliminar)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                    var result = await Task.Run(() => _tramitesProyectosServicio.EliminaLiberacionVF(tramiteEliminar));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerUsuariosPorInstanciaPadre")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(List<DatosUsuarioDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerUsuariosPorInstanciaPadre(InstanciaId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerCalendartioPeriodo")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(List<CalendarioPeriodoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult>  ObtenerCalendartioPeriodo(string bpin)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerCalendartioPeriodo(bpin));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerPresupuestalProyectosAsociados")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(List<PresupuestalProyectosAsociadosDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/ObtenerPresupuestalProyectosAsociados_Adicion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerPresupuestalProyectosAsociados_Adicion(TramiteId, InstanciaId));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/GetOrigenRecursosTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "OrigenRecursosDto", typeof(OrigenRecursosDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetOrigenRecursosTramite(int TramiteId)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.GetOrigenRecursosTramite(TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesProyectos/SetOrigenRecursosTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Inserta Origen Recursos", typeof(VigenciaFuturaResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramitesProyectosServicio.SetOrigenRecursosTramite(origenRecurso, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerModalidadContratacionVigenciasFuturas")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerModalidadContratacionVigenciasFuturas", typeof(int))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                ConfigurationManager.AppSettings["ObtenerModalidadContratacionVigenciasFuturas"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerModalidadContratacionVigenciasFuturas( ProyectoId, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar las autorizaciones para reprogramación.
        /// </summary>
        /// <param name="ObtenerAutorizacionesParaReprogramacion">Contiene filtro para proyectos a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/ObtenerAutorizacionesParaReprogramacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAutorizacionesParaReprogramacion(string Bpin, int TramiteId)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerAutorizacionesParaReprogramacion(Bpin, TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api asociar autorización .
        /// </summary>
        /// <param name="AsociarAutorizacionRVF">Contiene datos de la autorización a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/AsociarAutorizacionRVF")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociarAutorizacionRVF([FromBody] tramiteRVFAsociarproyecto reprogramacionDto)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.AsociarAutorizacionRVF(reprogramacionDto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para consultar las autorizaciones asociadas para reprogramación.
        /// </summary>
        /// <param name="ObtenerAutorizacionAsociada">Contiene filtro para proyectos a asociar</param>
        /// <returns>Resultado de la eliminación</returns>
        [Route("api/Tramites/ObtenerAutorizacionAsociada")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAutorizacionAsociada(int TramiteId)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesProyectosServicio.ObtenerAutorizacionAsociada(TramiteId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/EliminaReprogramacionVF")]
        public async Task<IHttpActionResult> EliminaReprogramacionVF(ReprogramacionDto tramiteEliminar)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var result = await Task.Run(() => _tramitesProyectosServicio.EliminaReprogramacionVF(tramiteEliminar));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}