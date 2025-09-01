using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesDistribucion;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.TramitesDistribucion
{
    public class TramitesDistribucionController : ApiController
    {
        private readonly ITramitesDistribucionServicio _tramitesDistribucionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TramitesDistribucionController(ITramitesDistribucionServicio tramitesDistribucionServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramitesDistribucionServicio = tramitesDistribucionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramitesDistribucion/ConsultarTramitesDistribucionAnteriores")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Tramites Distribucion Anteriores", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTramitesDistribucionAnteriores(Nullable<Guid> instanciaId)
        {
            var result = await Task.Run(() => _tramitesDistribucionServicio.ObtenerTramitesDistribucionAnteriores(instanciaId));

            return Ok(result);
        }
    }
}