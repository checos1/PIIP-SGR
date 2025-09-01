
namespace DNP.Backbone.Web.API.Controllers.AutorizacionNegocio
{
    using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
    using DNP.Backbone.Comunes.Excepciones;
    using System.Security.Principal;
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using System.Net;

    public class ReportesPIIPController : Base.BackboneBase
    {
        private readonly IReportesPIIPServicio _reportesPIIPServicio;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public ReportesPIIPController(IReportesPIIPServicio reportesPIIPServicio, IAutorizacionServicios autorizacionUtilidades) : base(autorizacionUtilidades)
        {
            _reportesPIIPServicio = reportesPIIPServicio;
            _autorizacionUtilidades = autorizacionUtilidades;

        }

        [Route("api/ObtenerDatosReportePIIPMga")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDNP, string idEntidades, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _reportesPIIPServicio.ObtenerDatosReportePIIP(idReporte, filtros, usuarioDNP, idEntidades, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            
        }
    }
}