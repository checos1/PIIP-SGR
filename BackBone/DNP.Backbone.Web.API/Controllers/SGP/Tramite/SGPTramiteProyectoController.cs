namespace DNP.Backbone.Web.API.Controllers.SGP.Tramite
{
    using Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Web.API.Controllers.Base;
    using Servicios.Interfaces.Autorizacion;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Servicios.Interfaces.SGP.Tramite;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class SGPTramiteProyectoController : BackboneBase
    {
        private readonly ISGPTramiteProyectoServicios _tramiteProyectoSGP;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="tramiteProyectoSGPServicios">Instancia de tramite proyecto Servicios</param>
        public SGPTramiteProyectoController(ISGPTramiteProyectoServicios tramiteProyectoSGPServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _tramiteProyectoSGP = tramiteProyectoSGPServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [HttpGet]
        [Route("api/TramiteProyectoSGP/ObtenerProyectosTramiteNegocio")]
        public async Task<IHttpActionResult> ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteProyectoSGP.ObtenerProyectosTramiteNegocio(TramiteId, User.Identity.Name, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteProyectoSGP/GuardarProyectosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _tramiteProyectoSGP.GuardarProyectosTramiteNegocio(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/TramiteProyectoSGP/ValidacionProyectosTramiteNegocio")]
        public async Task<IHttpActionResult> ValidacionProyectosTramiteNegocio(int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteProyectoSGP.ValidacionProyectosTramiteNegocio(TramiteId, User.Identity.Name, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}