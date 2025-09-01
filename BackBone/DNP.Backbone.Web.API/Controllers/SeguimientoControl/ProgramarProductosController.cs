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
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class ProgramarProductosController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IProgramarProductosServicio _programarProductosServicio;
        private readonly IFlujoServicios _flujoServicios;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="desagregarEdtServicio">Instancia de servicios de Negocio Servicios</param>
        public ProgramarProductosController(
            IAutorizacionServicios autorizacionUtilidades,
            IProgramarProductosServicio desagregarEdtServicio)
            : base(autorizacionUtilidades)
        {
            _programarProductosServicio = desagregarEdtServicio;
        }

        #region Get

        [Route("api/seguimientoControl/ProgramarProductos/ObtenerListadoObjProdNiveles")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNiveles(string bpin, string usuarioDNP)
        {
            try
            {
                return Ok(await _programarProductosServicio.ObtenerListadoObjProdNiveles(bpin, usuarioDNP));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion

        [Route("api/seguimientoControl/ProgramarProductos/GuardarProgramarProducto")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProgramarProducto(ProgramarProductoDto ProgramarProducto)
        {
            try
            {
                var result = await Task.Run(() => _programarProductosServicio.GuardarProgramarProducto(ProgramarProducto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}