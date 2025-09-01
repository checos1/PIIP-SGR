using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.ReporteAvance;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SeguimientoControl
{
    public class DesagregarEdtController : ApiController
    {

        private readonly IDesagregarEdtServicio _DesagregarEdtServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        #region Costructor
        public DesagregarEdtController(IDesagregarEdtServicio IDesagregarEdtServicio,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _DesagregarEdtServicio = IDesagregarEdtServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
        #endregion

        /// <summary>
        /// Método que obtiene el listado de Objetivos - Productos - Niveles - Actividades
        /// </summary>
        /// <param name="guiMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeguimientoControl/DesagregarEdt/ObtenerListadoObjProdNiveles")]
        [SwaggerResponse(HttpStatusCode.OK, "Desagregar EDT - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNiveles([FromBody] ConsultaObjetivosProyecto ProyectosDto)
        {
            var result = await Task.Run(() => _DesagregarEdtServicio.ObtenerListadoObjProdNiveles(ProyectosDto));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/SeguimientoControl/DesagregarEdt/RegistrarNivel")]
        [SwaggerResponse(HttpStatusCode.OK, "Desagregar EDT - Registra nuevos niveles", typeof(List<RegistroEntregable>))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarNivel([FromBody] RegistroModel NivelesNuevos)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            try {
                var usuario = RequestContext.Principal.Identity.Name;
                var response = await Task.Run(() => _DesagregarEdtServicio.RegistrarNivel(usuario, NivelesNuevos));
                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
               return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/SeguimientoControl/DesagregarEdt/EliminarNivel")]
        [SwaggerResponse(HttpStatusCode.OK, "Desagregar EDT - Eliminar niveles", typeof(List<RegistroEntregable>))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarNivel([FromBody] RegistroModel NivelesNuevos)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var response = await Task.Run(() => _DesagregarEdtServicio.EliminarNivel(usuario, NivelesNuevos));
                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }


        /// <summary>
        /// tratamiento para HTTP Status Code 404
        /// </summary>        
        /// <returns>IHttpActionResult</returns>
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

        [Route("api/seguimientoControl/AvanceFinanciero/ObtenerPreguntasAvanceFinanciero")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Preguntas Avance Financiero", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid)
        {
            var result = await Task.Run(() => _DesagregarEdtServicio.ObtenerPreguntasAvanceFinanciero(instancia, proyectoid, bpin, nivelid));

            return Ok(result);
        }        

        [Route("api/seguimientoControl/AvanceFinanciero/GuardarPreguntasAvanceFinanciero")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasAvanceFinanciero(List<PreguntasReporteAvanceFinancieroDto> objscDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<List<PreguntasReporteAvanceFinancieroDto>>();
                parametrosGuardar.Contenido = objscDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _DesagregarEdtServicio.GuardarPreguntasAvanceFinanciero(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/seguimientoControl/AvanceFinanciero/ObtenerAvanceFinanciero")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Avance Financiero", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId)
        {
            var result = await Task.Run(() => _DesagregarEdtServicio.ObtenerAvanceFinanciero(instancia, proyectoid, bpin, vigenciaId, periodoPeriodicidadId));

            return Ok(result);
        }

        [Route("api/seguimientoControl/AvanceFinanciero/GuardarAvanceFinanciero")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarAvanceFinanciero(AvanceFinancieroDto objscDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<AvanceFinancieroDto>();
                parametrosGuardar.Contenido = objscDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _DesagregarEdtServicio.GuardarAvanceFinanciero(parametrosGuardar, RequestContext.Principal.Identity.Name));

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
    }
}
