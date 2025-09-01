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

    public class ReporteAvanceProductoController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IReporteAvanceProductoServicio _reporteAvanceProductoServicio;
        private readonly IFlujoServicios _flujoServicios;

        public ReporteAvanceProductoController(
            IAutorizacionServicios autorizacionUtilidades,
            IReporteAvanceProductoServicio reporteAvanceProductoServicio)
            : base(autorizacionUtilidades)
        {
            _reporteAvanceProductoServicio = reporteAvanceProductoServicio;
        }

        #region Get

        [Route("api/ReporteAvanceProducto/ConsultarAvanceMetaProducto")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            try
            {
                var result = await Task.Run(() => _reporteAvanceProductoServicio.ConsultarAvanceMetaProducto(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion


        #region Post


        [Route("api/ReporteAvanceProducto/ActualizarAvanceMetaProducto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto)
        {
            try
            {
                var result = await Task.Run(() => _reporteAvanceProductoServicio.ActualizarAvanceMetaProducto(IndicadorDto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));


            }
        }

        #endregion

    }
}