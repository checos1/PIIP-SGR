using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.AutorizacionNegocio
{
    using DNP.Backbone.Comunes.Excepciones;
    using Servicios.Interfaces.Autorizacion;
    using System.Net;
    using System.Net.Http;

    public class AutorizacionController : ApiController
    {
        private readonly IAutorizacionServicios _autorizacionServicios;

        public AutorizacionController(IAutorizacionServicios autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }

        /// <summary>
        /// Obtiene una Cuenta de Usuario por nombre da cuenta
        /// </summary>
        /// <returns></returns>
        [Route("api/Autorizacion/ObtenerCuentaUsuario")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCuentaUsuario(string nomeCuenta)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerCuentaUsuario(nomeCuenta, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Autorizacion/ObtenerUsuarioPorCorreoDNP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuarioPorCorreoDNP(string correo)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuarioPorCorreoDNP(correo, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Autorizacion/ObtenerUsuarioPorIdUsuarioDnp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuarioPorIdUsuarioDnp(string idUsuarioDnp)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerUsuarioPorIdUsuarioDnp(idUsuarioDnp));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Autorizacion/ObtenerListaEntidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaEntidad(string usuarioDnp, string objetoNegocioId = null)
        {
            try
            {
                var result = await Task.Run(() => _autorizacionServicios.ObtenerListaEntidad(usuarioDnp, objetoNegocioId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Autorizacion/validarPermisoInactivarUsuario")]
        [HttpGet]
        public async Task<IHttpActionResult> validarPermisoInactivarUsuario(string usuarioDnp, string usuarioDnpEliminar)
        {
            var result = await Task.Run(() => _autorizacionServicios.validarPermisoInactivarUsuario(usuarioDnp, usuarioDnpEliminar));
            return Ok(result);
        }

        [Route("api/Autorizacion/ObtnerEntidadesPorSector")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtnerEntidadesPorSector(int sectorId, string tipoEntidad, string usuarioDnp)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtnerEntidadesPorSector(sectorId, tipoEntidad, usuarioDnp));
            return Ok(result);
        }


        [Route("api/Autorizacion/ObtenerEncabezadoListadoReportesPIIP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEncabezadoListadoReportesPIIP(string usuarioDnp)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerEncabezadoListadoReportesPIIP(usuarioDnp));
            return Ok(result);
        }

        [Route("api/Autorizacion/ObtenerListadoReportesPIIP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListadoReportesPIIP(string usuarioDnp, string idRoles)
        {
            //List<Guid> idRolesX = new List<Guid>();
            //idRolesX.Add(Guid.Parse("9DB2FA6E-1D6D-4E85-98D3-76FF98618279"));
            var result = await Task.Run(() => _autorizacionServicios.ObtenerListadoReportesPIIP(usuarioDnp, idRoles));
            return Ok(result);
        }

        [Route("api/Autorizacion/ObtenerFiltrosReportesPIIP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFiltrosReportesPIIP(Guid idReporte, string usuarioDnp)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerFiltrosReportesPIIP(idReporte, usuarioDnp));
            return Ok(result);
        }

        [Route("api/Autorizacion/ObtenerDatosReportePIIP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDnp, string idEntidades)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerDatosReportePIIP(idReporte, filtros, usuarioDnp, idEntidades));
            return Ok(result);
        }
    }
}