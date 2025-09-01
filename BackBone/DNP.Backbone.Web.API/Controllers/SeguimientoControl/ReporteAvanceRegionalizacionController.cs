namespace DNP.Backbone.Web.API.Controllers
{
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

    public class ReporteAvanceRegionalizacionController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IReporteAvanceRegionalizacionServicio _reporteAvanceRegionalizacionServicio;
        private readonly IFlujoServicios _flujoServicios;

        public ReporteAvanceRegionalizacionController(
            IAutorizacionServicios autorizacionUtilidades,
            IReporteAvanceRegionalizacionServicio reporteAvanceRegionalizacionServicio)
            : base(autorizacionUtilidades)
        {
            _reporteAvanceRegionalizacionServicio = reporteAvanceRegionalizacionServicio;
        }

        #region Get

        [Route("api/ReporteAvanceRegionalizacion/ConsultarAvanceRegionalizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarAvanceRegionalizacion(Guid? instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            try
            {
                var result = await Task.Run(() => _reporteAvanceRegionalizacionServicio.ConsultarAvanceRegionalizacion(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ReporteAvanceRegionalizacion/ConsultarResumenAvanceRegionalizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarResumenAvanceRegionalizacion(Guid? instanciaId, int proyectoId, string codigoBpin)
        {
            try
            {
                var result = await Task.Run(() => _reporteAvanceRegionalizacionServicio.ConsultarResumenAvanceRegionalizacion(instanciaId, proyectoId, codigoBpin, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion


        //#region Post


        [Route("api/ReporteAvanceRegionalizacion/GuardarAvanceRegionalizacion")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarAvanceMetaProducto(AvanceRegionalizacionDto IndicadorDto)
        {
            try
            {
                var result = await Task.Run(() => _reporteAvanceRegionalizacionServicio.GuardarAvanceRegionalizacion(IndicadorDto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ReporteAvanceRegionalizacion/ObtenerDetalleRegionalizacionProgramacionSeguimiento")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json, string usuarioDNP)
        {
            try
            {
                return Ok(await _reporteAvanceRegionalizacionServicio.ObtenerDetalleRegionalizacionProgramacionSeguimiento(json, usuarioDNP));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }



        //#endregion

    }
}