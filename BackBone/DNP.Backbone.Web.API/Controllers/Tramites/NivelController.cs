using System;

namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Comunes.Properties;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Web.API.Controllers.Base;
    using System.Net.Http.Headers;
    using System.Web.Management;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Servicios.Interfaces.Nivel;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class NivelController : BackboneBase
    {
        private readonly INivelServicios _nivelServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="tramiteServicios">Instancia de servicios de trámites</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public NivelController(INivelServicios nivelServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _nivelServicios = nivelServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [HttpGet]
        [Route("api/Nivel/ObtenerPorIdPadreIdNivelTipo")]
        public async Task<IHttpActionResult> ObtenerPorIdPadreIdNivelTipo([FromUri] Guid? idPadre, [FromUri] string claveTipoNivel)
        {
            var result = await Task.Run(() => _nivelServicios.ObtenerPorIdPadreIdNivelTipo(idPadre, claveTipoNivel, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }
    }
}