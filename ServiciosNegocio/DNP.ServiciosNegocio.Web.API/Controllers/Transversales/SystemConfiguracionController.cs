using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
namespace DNP.ServiciosNegocio.Web.API.Controllers.Transversales
{
    public class SystemConfiguracionController : ApiController
    {
        private readonly ISystemConfiguracionServicios _SystemConfiguracionServicios;
        #region Costructor
        public SystemConfiguracionController(ISystemConfiguracionServicios ISystemConfiguracionServicios,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _SystemConfiguracionServicios = ISystemConfiguracionServicios;
        }
        #endregion
        [Route("api/SystemConfiguracion/ObtenerSystemConfiguracion")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener SystemConfiguracion", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSystemConfiguracion(string VariableKey, string Separador)
        {
            var result = await Task.Run(() => _SystemConfiguracionServicios.ObtenerSystemConfiguracion(VariableKey, Separador));
            return Ok(result);
        }
    }
}