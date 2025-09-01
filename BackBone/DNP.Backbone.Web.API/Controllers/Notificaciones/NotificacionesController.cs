namespace DNP.Backbone.Web.API.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Excepciones;
    using Servicios.Interfaces;

    public class NotificacionesController : ApiController
    {
        private readonly IBackboneServicios _backboneServicios;

        public NotificacionesController(IBackboneServicios backboneServicios)
        {
            _backboneServicios = backboneServicios;
        }

        [Route("api/Proyectos/ObtenerNotificacionesPorResponsable")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerNotificacionesPorResponsable(string usuarioResponsable)
        {
            var result = await Task.Run(() => _backboneServicios.ConsultarNotificacionPorResponsable(usuarioResponsable));
            return Ok(result);
        }

        [Route("api/Notificaciones/NotificarUsuarios")]
        [HttpPost]
        public async Task<IHttpActionResult> NotificarUsuarios(List<ParametrosCrearNotificacionFlujoDto> parametros, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _backboneServicios.NotificarUsuarios(parametros, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}