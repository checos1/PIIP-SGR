using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
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
    public class ProgramarActividadesController : ApiController
    {

        private readonly IProgramarActividadesServicio _ProgramarActividadesServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        #region Costructor
        public ProgramarActividadesController(IProgramarActividadesServicio ProgramarActividadesServicio,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _ProgramarActividadesServicio = ProgramarActividadesServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
        #endregion

        /// <summary>
        /// Método que obtiene el listado de Objetivos - Productos - Niveles - Actividades
        /// </summary>
        /// <param name="guiMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeguimientoControl/ProgramarActividades/ObtenerListadoObjProdNiveles")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNiveles([FromBody] ConsultaObjetivosProyecto ProyectosDto)
        {
            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerListadoObjProdNiveles(ProyectosDto));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/SeguimientoControl/ProgramarActividades/ObtenerListadoObjProdNivelesXReporte")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNivelesXReporte([FromBody] ConsultaObjetivosProyecto ProyectosDto)
        {
            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerListadoObjProdNivelesXReporte(ProyectosDto));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerIndicadoresPoliticas")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerIndicadoresPoliticas([FromBody] ConsultaObjetivosProyecto ProyectosDto)
        {
            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerIndicadoresPoliticas(ProyectosDto));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/SeguimientoControl/ProgramarActividades/ObtenerCalendarioPeriodo")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerCalendarioPeriodo([FromBody] ConsultaObjetivosProyecto ProyectosDto)
        {
            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerCalendarioPeriodo(ProyectosDto));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/seguimientoControl/ProgramarActividades/ActividadProgramacionSeguimientoPeriodosValores")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ActividadProgramacionSeguimientoPeriodosValores([FromBody] List<VigenciaEntregableDto> parametros)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _ProgramarActividadesServicio.ActividadProgramacionSeguimientoPeriodosValores(usuario, parametros));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/seguimientoControl/ProgramarActividades/ActividadReporteSeguimientoPeriodosValores")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ActividadReporteSeguimientoPeriodosValores([FromBody] ReporteSeguimiento parametros)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _ProgramarActividadesServicio.ActividadReporteSeguimientoPeriodosValores(usuario, parametros));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/seguimientoControl/ProgramarIndicadorPolitica/IndicadorPoliticaSeguimientoPeriodosValores")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> IndicadorPoliticaSeguimientoPeriodosValores([FromBody] ReporteIndicadorPoliticas parametros)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _ProgramarActividadesServicio.IndicadorPoliticaSeguimientoPeriodosValores(usuario, parametros));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        /// <summary>
        /// Método que registra / edita actividad en programar actividades
        /// </summary>
        /// <returns></returns>
        [Route("api/SeguimientoControl/ProgramarActividades/EditarProgramarActividad")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Estado de transacción", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> EditarProgramarActividad(ProgramarActividadesDto actividad)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _ProgramarActividadesServicio.EditarProgramarActividad(usuario, actividad));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
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

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerFocalizacionProgramacionSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Focalizacion Programacion Seguimiento", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta));

            return Ok(result);
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/GuardarFocalizacionProgramacionSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionProgramacionSeguimiento(FocalizacionProgramacionSeguimientoDto objFocalizacionProgramacionSeguimientoDto)
        {
            try
            {
                /*
				var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
					RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
					ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
				if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
				*/
                var parametrosGuardar = new ParametrosGuardarDto<FocalizacionProgramacionSeguimientoDto>();
                parametrosGuardar.Contenido = objFocalizacionProgramacionSeguimientoDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _ProgramarActividadesServicio.GuardarFocalizacionProgramacionSeguimiento(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerCruceProgramacionSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Cruce Programacion Seguimiento", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid)
        {
            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerCruceProgramacionSeguimiento(instanciaid, proyectoid));

            return Ok(result);
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/GuardarCruceProgramacionSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCruceProgramacionSeguimiento(FocalizacionCrucePoliticaSeguimientoDto objFocalizacionCrucePoliticaSeguimientoDto)
        {
            try
            {
                /*
				var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
					RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
					ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
				if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
				*/
                var parametrosGuardar = new ParametrosGuardarDto<FocalizacionCrucePoliticaSeguimientoDto>();
                parametrosGuardar.Contenido = objFocalizacionCrucePoliticaSeguimientoDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _ProgramarActividadesServicio.GuardarCruceProgramacionSeguimiento(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerFocalizacionProgramacionSeguimientoDetalle")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Focalizacion Programacion Seguimiento", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ProgramarActividadesServicio.ObtenerFocalizacionProgramacionSeguimientoDetalle(parametros));

            return Ok(result);
        }

    }
}
