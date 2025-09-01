using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.ReportesPIIP;

    public class ReportesPIIPController : ApiController
    {
        private readonly IReportesPIIPServicio _reportesPIIPServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ReportesPIIPController(IReportesPIIPServicio reportesPIIPServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _reportesPIIPServicio = reportesPIIPServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/ObtenerDatosReportePIIPMga")] 
        [SwaggerResponse(HttpStatusCode.OK, "Retorna la informacion del reporte solicitado", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string idEntidades)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                   ConfigurationManager.AppSettings["GenerarReportesPIIP"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _reportesPIIPServicio.ObtenerDatosReportePIIP(idReporte, filtros, idEntidades));

            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

    }
}