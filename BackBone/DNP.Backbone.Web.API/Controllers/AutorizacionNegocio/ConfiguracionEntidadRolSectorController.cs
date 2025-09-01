using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;

namespace DNP.Backbone.Web.API.Controllers.AutorizacionNegocio
{
    using DNP.Backbone.Dominio.Filtros;
    using Servicios.Interfaces.Autorizacion;

    public class ConfiguracionEntidadRolSectorController : ApiController
    {
        private readonly IAutorizacionServicios _autorizacionServicios;

        public ConfiguracionEntidadRolSectorController(IAutorizacionServicios autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }

        [Route("api/AutorizacionNegocio/ObtenerConfiguracionesRolSector")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerConfiguracionesRolSector(string usuarioDnp, string nombreAplicacion)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idObtenerConfiguracionesRolSector"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.ObtenerConfiguracionesRolSector(usuarioDnp));
            return Ok(result);
        }

        [Route("api/AutorizacionNegocio/ObtenerRolesPorEntidadTerritorial")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerRolesPorEntidadTerritorial(string usuarioDnp, string nombreAplicacion, Guid idEntidadTerritorial)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idObtenerRolesPorEntidadTerritorial"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.ObtenerRolesPorEntidadTerritorial(usuarioDnp, idEntidadTerritorial));
            return Ok(result);
        }

        [Route("api/AutorizacionNegocio/ObtenerSectoresPorEntidadTerritorial")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSectoresPorEntidadTerritorial(string usuarioDnp, string nombreAplicacion, Guid idEntidadTerritorial)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idObtenerSectoresPorEntidadTerritorial"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.ObtenerSectoresPorEntidadTerritorial(usuarioDnp, idEntidadTerritorial));
            return Ok(result);
        }

        [Route("api/AutorizacionNegocio/ObtenerEntidadesPorSectorTerritorial")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEntidadesPorSectorTerritorial(string usuarioDnp, string nombreAplicacion, Guid idEntidadTerritorial, Guid idSector)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idObtenerEntidadesPorSectorTerritorial"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.ObtenerEntidadesPorSectorTerritorial(usuarioDnp, idEntidadTerritorial, idSector));
            return Ok(result);
        }

        [Route("api/AutorizacionNegocio/GuardarConfiguracionRolSector")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarConfiguracionRolSector(PeticionConfiguracionRolSectorDto peticion)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idGuardarConfiguracionRolSector"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.GuardarConfiguracionRolSectorAsync(peticion));
            return Ok(result);
        }

        [Route("api/AutorizacionNegocio/EditarConfiguracionRolSector")]
        [HttpPut]
        public async Task<IHttpActionResult> EditarConfiguracionRolSector(PeticionConfiguracionRolSectorDto peticion)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idEditarConfiguracionRolSector"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.EditarConfiguracionRolSector(peticion));
            return Ok(result);
        }

        [Route("api/AutorizacionNegocio/CambiarEstadoConfiguracionRolSector")]
        [HttpPut]
        public async Task<IHttpActionResult> CambiarEstadoConfiguracionRolSector(PeticionCambioEstadoConfiguracionDto peticion)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                               ConfigurationManager.AppSettings["idCambiarEstadoConfiguracionRolSector"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _autorizacionServicios.CambiarEstadoConfiguracionRolSector(peticion));
            return Ok(result);
        }

        /// <summary>
        /// Obtener opciones.
        /// </summary>
        /// <returns>Obtiene las opciones filtradas.</returns>
        [Route("api/AutorizacionNegocio/ObtenerOpcionesConFiltro")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerOpcionesConFiltro([FromUri] string idAplicacion)
        {
            var respuestaAutorizacion = _autorizacionServicios.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                              ConfigurationManager.AppSettings["idCambiarEstadoConfiguracionRolSector"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result =
                await
                    Task.Run(() => _autorizacionServicios.ObtenerOpcionesConFiltro(idAplicacion, RequestContext.Principal.Identity.Name));
            return Ok(result);
        }
    }
}