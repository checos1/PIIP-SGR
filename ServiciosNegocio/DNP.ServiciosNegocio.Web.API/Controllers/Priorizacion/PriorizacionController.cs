using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Priorizacion
{
    public class PriorizacionController : ApiController
    {
        private readonly IPriorizacionServicio _priorizacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public PriorizacionController(IPriorizacionServicio priorizacionServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _priorizacionServicio = priorizacionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Priorizacion/ConsultarPriorizacionPorBPINs")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna priorizacion", typeof(PriorizacionDatosBasicosDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                   ConfigurationManager.AppSettings["ConsultarPriorizacionPorBPINs"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ////ValidarParametros(bpins);
            //bpins.TokenAutorizacion = Request.Headers.Authorization.Parameter;

            var result = await Task.Run(() => _priorizacionServicio.ConsultarProyectosPorBPINs(bpins));

            return Responder(result);
        }

        [Route("api/priorizacion/ObtenerRegistroPriorizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna priorizacion", typeof(InstanciaPriorizacionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerRegistroPriorizacion(ObjetoNegocio objetoNegocio)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                  RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                  ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                  ConfigurationManager.AppSettings["ConsultarPriorizacionPorBPINs"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _priorizacionServicio.ObtenerRegistroPriorizacion(objetoNegocio));

            return Responder(result);
        }

        [Route("api/Priorizacion/ConsultarFuentesSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Fuentes SGR", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesSGR(string bpin, Nullable<Guid> instanciaId)
        {
            var result = await Task.Run(() => _priorizacionServicio.ObtenerFuentesSGR(bpin, instanciaId));

            return Ok(result);
        }

        [Route("api/Priorizacion/RegistrarFuentesSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar fuentes SGR", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarFuentesSGR([FromBody] List<EtapaSGRDto> json)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var response = await Task.Run(() => _priorizacionServicio.RegistrarViabilidadFuentesSGR(json, usuario));
                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/Priorizacion/ConsultarFuentesNoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Fuentes SGR", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesNoSGR(string bpin, Nullable<Guid> instanciaId)
        {
            var result = await Task.Run(() => _priorizacionServicio.ObtenerFuentesNoSGR(bpin, instanciaId));

            return Ok(result);
        }

        //[Route("api/Priorizacion/ObtenerPriorizacionProyecto")]
        //[SwaggerResponse(HttpStatusCode.OK, "Obtener priorizacion proyecto", typeof(HttpResponseMessage))]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId)
        //{
        //    var result = await Task.Run(() => _priorizacionServicio.ObtenerPriorizacionProyecto(instanciaId));
        //    return Ok(result);
        //}

        //[Route("api/Priorizacion/ObtenerAprobacionProyecto")]
        //[SwaggerResponse(HttpStatusCode.OK, "Obtener aprobación proyecto", typeof(HttpResponseMessage))]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId)
        //{
        //    var result = await Task.Run(() => _priorizacionServicio.ObtenerAprobacionProyecto(instanciaId));
        //    return Ok(result);
        //}

        [Route("api/Priorizacion/RegistrarFuentesNoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar fuentes no SGR", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarFuentesNoSGR([FromBody] List<EtapaNoSGRDto> json)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var response = await Task.Run(() => _priorizacionServicio.RegistrarViabilidadFuentesNoSGR(json, usuario));
                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/Priorizacion/ConsultarResumenFuentesCostos")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Fuentes Costos", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenFuentesCostos(string bpin, Nullable<Guid> instanciaId)
        {
            var result = await Task.Run(() => _priorizacionServicio.ObtenerResumenFuentesCostos(bpin, instanciaId));

            return Ok(result);
        }

        [Route("api/Priorizacion/RegistrarDatosCofinanciadorFuentesNoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar datos adicionales cofinanciador fuentes no SGR", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR([FromBody] DatosAdicionalesCofinanciadorDto json)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var response = await Task.Run(() => _priorizacionServicio.RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(json, usuario));
                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/Priorizacion/ConsultarDatosCofinanciadorFuentesNoSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Fuentes SGR", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, Nullable<int> vigencia, Nullable<int> vigenciaFuente)
        {
            var result = await Task.Run(() => _priorizacionServicio.ObtenerDatosAdicionalesCofinanciadorNoSGR(bpin, vigencia, vigenciaFuente));

            return Ok(result);
        }

        [Route("api/Priorizacion/GuardarPermisosPriorizionProyectoDetalleSGR")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Permisos Priorizion Proyecto Detalle SGR", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto)
        {
            try
            {
                await Task.Run(() => _priorizacionServicio.GuardarPermisosPriorizionProyectoDetalleSGR(proyectoPriorizacionDetalleDto, RequestContext.Principal.Identity.Name));

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

        #region Respuestas Servicio        
        private IHttpActionResult Responder(object listaProyecto)
        {
            return listaProyecto != null ? Ok(listaProyecto) : CrearRespuestaNoFound();
        }        

        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        private IHttpActionResult CrearRespuestaError(string message)
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = message
            };

            return ResponseMessage(respuestaHttp);
        }
        #endregion
    }
}
