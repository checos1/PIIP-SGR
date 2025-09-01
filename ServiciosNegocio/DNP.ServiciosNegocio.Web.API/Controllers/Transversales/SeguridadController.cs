
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
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
    public class SeguridadController : ApiController
    {
        private readonly ISeguridadServicio _SeguridadServicio;

        #region Costructor
        public SeguridadController(ISeguridadServicio ISeguridadServicio,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _SeguridadServicio = ISeguridadServicio;

        }
        #endregion


        [Route("api/Seguridad/PermisosAccionPaso")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Permisos Paso", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> PermisosAccionPaso(AccionFlujoDto accionFlujoDto)
        {
            var result = await Task.Run(() => _SeguridadServicio.PermisosAccionPaso(accionFlujoDto));

            return Ok(result);
        }

    }
}