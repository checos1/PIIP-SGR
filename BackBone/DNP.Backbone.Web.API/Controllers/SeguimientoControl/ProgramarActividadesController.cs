namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.SeguimientoControl;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class ProgramarActividadesController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IProgramarActividadesServicio _programarActividadesServicio;
        private readonly IFlujoServicios _flujoServicios;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="desagregarEdtServicio">Instancia de servicios de Negocio Servicios</param>
        public ProgramarActividadesController(
            IAutorizacionServicios autorizacionUtilidades,
            IProgramarActividadesServicio desagregarEdtServicio)
            : base(autorizacionUtilidades)
        {
            _programarActividadesServicio = desagregarEdtServicio;
        }

        #region Get

        [Route("api/seguimientoControl/ProgramarActividades/ObtenerListadoObjProdNiveles")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto parametros)
        {
            try
            {
                parametros.UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerListadoObjProdNiveles(parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/ProgramarActividades/ObtenerCalendarioPeriodo")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto parametros)
        {
            try
            {
                parametros.UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerCalendarioPeriodo(parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Post
        [Route("api/seguimientoControl/ProgramarActividades/ObtenerListadoObjProdNivelesXReporte")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto parametros)
        {
            try
            {
                parametros.UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerListadoObjProdNivelesXReporte(parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerIndicadoresPoliticas")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto parametros)
        {
            try
            {
                parametros.UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerIndicadoresPoliticas(parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/ProgramarActividades/EditarProgramarActividad")]
        [HttpPost]
        public async Task<IHttpActionResult> EditarProgramarActividad(ProgramarActividadesDto Actividad)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.EditarProgramarActividad(UsuarioDNP, Actividad));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/ProgramarActividades/ActividadProgramacionSeguimientoPeriodosValores")]
        [HttpPost]
        public async Task<IHttpActionResult> ActividadProgramacionSeguimientoPeriodosValores(List<VigenciaEntregable> parametros)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ActividadProgramacionSeguimientoPeriodosValores(UsuarioDNP, parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/ProgramarActividades/ActividadReporteSeguimientoPeriodosValores")]
        [HttpPost]
        public async Task<IHttpActionResult> ActividadReporteSeguimientoPeriodosValores(ReporteSeguimiento parametros)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ActividadReporteSeguimientoPeriodosValores(UsuarioDNP, parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/ProgramarIndicadorPolitica/IndicadorPoliticaSeguimientoPeriodosValores")]
        [HttpPost]
        public async Task<IHttpActionResult> IndicadorPoliticaSeguimientoPeriodosValores(ReporteIndicadorPoliticas parametros)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.IndicadorPoliticaSeguimientoPeriodosValores(UsuarioDNP, parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerFocalizacionProgramacionSeguimiento")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta)
        {
            try
            {
                string usuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerFocalizacionProgramacionSeguimiento(parametroConsulta, usuarioDNP));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/GuardarFocalizacionProgramacionSeguimiento")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionProgramacionSeguimiento(FocalizacionProgramacionSeguimientoDto objFocalizacionProgramacionSeguimientoDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _programarActividadesServicio.GuardarFocalizacionProgramacionSeguimiento(objFocalizacionProgramacionSeguimientoDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerCruceProgramacionSeguimiento")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid)
        {
            try
            {
                string usuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerCruceProgramacionSeguimiento(instanciaid, proyectoid, usuarioDNP));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/GuardarCrucePoliticasSeguimiento")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCrucePoliticasSeguimiento(FocalizacionCrucePoliticaSeguimientoDto objFocalizacionCrucePoliticaSeguimientoDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _programarActividadesServicio.GuardarCrucePoliticasSeguimiento(objFocalizacionCrucePoliticaSeguimientoDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeguimientoControl/FocalizacionIndicadores/ObtenerFocalizacionProgramacionSeguimientoDetalle")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros, string usuarioDNP)
        {
            try
            {
                //string usuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _programarActividadesServicio.ObtenerFocalizacionProgramacionSeguimientoDetalle(parametros, usuarioDNP));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Delete

        #endregion

    }
}