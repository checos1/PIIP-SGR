using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Monitoreo;
using DNP.Backbone.Web.API.Controllers.Base;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers
{
    public class AlertasGeneradasController : BackboneBase
    {
        private readonly IAlertasGeneradasServicios _alertasGeneradasServicios;

        public AlertasGeneradasController(IAlertasGeneradasServicios alertasGeneradasServicios, IAutorizacionServicios autorizacionUtilidades) 
            : base(autorizacionUtilidades)
        {
            _alertasGeneradasServicios = alertasGeneradasServicios;
        }

        /// <summary>
        /// Api para obtención de alertas generadas.
        /// </summary>
        /// <param name="alertasFilterdto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de alertas generadas.</returns>
        [Route("api/Monitoreo/AlertasGeneradas")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMapColumnas(AlertasGeneradasFiltroDto alertasFilterdto)
        {
            try
            {
                if (!await ValidarParametrosRequest(alertasFilterdto.ParametrosDto).ConfigureAwait(false))
                    return ResponseMessage(RespuestaAutorizacion);

                var result = await _alertasGeneradasServicios.ObtenerAlertasGeneradas(alertasFilterdto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


    }
}