using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Flujos
{
    using Servicios.Interfaces.Autorizacion;
    using Servicios.Interfaces.ServiciosNegocio;

    public class MenuContextualController : ApiController
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public MenuContextualController(IFlujoServicios flujoServicios, IAutorizacionServicios autorizacionUtilidades)
        {
            _flujoServicios = flujoServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Flujos/ObtenerFlujoPorInstanciaTarea")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFlujoPorInstanciaTarea(string nombreAplicacion, string usuarioDnp, Guid idInstancia)
        {
            if (!ValidarParametros(nombreAplicacion, usuarioDnp, idInstancia))
            {
                return Ok(HttpStatusCode.BadRequest);
            }
            var respuestaAutorizacion =await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idObtenerFlujoPorInstanciaTarea"]).ConfigureAwait(false);

            if (!respuestaAutorizacion.IsSuccessStatusCode)
            {
                return ResponseMessage(respuestaAutorizacion);
            }

            var result = await _flujoServicios.ObtenerFlujoPorInstanciaTarea(usuarioDnp, idInstancia).ConfigureAwait(false);
            return Ok(result);
        }

        #region  MÉTODOS PRIVADOS

        private static bool ValidarParametros(string nombreAplicacion, string usuarioDnp, Guid idInstancia)
        {
            if ((string.IsNullOrEmpty(nombreAplicacion)) || (string.IsNullOrWhiteSpace(nombreAplicacion)))
            {
                return false;
            }

            if (string.IsNullOrEmpty(usuarioDnp) || string.IsNullOrWhiteSpace(usuarioDnp))
            {
                return false;
            }

            return idInstancia != Guid.Empty;
        }

        #endregion

    }
}