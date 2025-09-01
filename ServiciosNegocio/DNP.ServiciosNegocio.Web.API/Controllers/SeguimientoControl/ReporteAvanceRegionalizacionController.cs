using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SeguimientoControl
{
    public class ReporteAvanceRegionalizacionController : ApiController
    {
        private readonly IReporteAvanceRegionalizacionServicio _ReporteAvanceRegionalizacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        #region Costructor
        public ReporteAvanceRegionalizacionController(IReporteAvanceRegionalizacionServicio ReporteAvanceRegionalizacionServicio,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _ReporteAvanceRegionalizacionServicio = ReporteAvanceRegionalizacionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
        #endregion


        [Route("api/SeguimientoControl/ReporteAvanceRegionalizacion/ConsultarReporteAvanceRegionalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Avance Regionalizacion", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarReporteAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            var result = await Task.Run(() => _ReporteAvanceRegionalizacionServicio.ConsultarAvanceRegionalizacion(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad));

            return Ok(result);
        }

        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

    }
}