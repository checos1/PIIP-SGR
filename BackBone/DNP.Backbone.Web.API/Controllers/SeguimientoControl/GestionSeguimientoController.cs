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
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class GestionSeguimientoController : Base.BackboneBase
    {
        private readonly IGestionSeguimientoServicio _gestionProyectoSeguimiento;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="desagregarEdtServicio">Instancia de servicios de Negocio Servicios</param>
        public GestionSeguimientoController(
            IAutorizacionServicios autorizacionUtilidades,
            IGestionSeguimientoServicio gestionProyectoSeguimientoService)
            : base(autorizacionUtilidades)
        {
            _gestionProyectoSeguimiento = gestionProyectoSeguimientoService;
        }

        #region Get

        [Route("api/gestionSeguimiento/UnidadesMedida")]
        [HttpGet]
        public async Task<IHttpActionResult> UnidadesMedida()
        {
            try
            {
                return Ok(await _gestionProyectoSeguimiento.UnidadesMedida(UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Post
        [Route("api/gestionSeguimiento/ObtenerErroresSeguimiento")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerErrores([FromBody] GestionProyectoDto proyecto)
        {
            try
            {
                proyecto.IdUsuario = UsuarioLogadoDto.IdUsuario;
                return Ok(await _gestionProyectoSeguimiento.ObtenerErroresSeguimiento(proyecto));
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