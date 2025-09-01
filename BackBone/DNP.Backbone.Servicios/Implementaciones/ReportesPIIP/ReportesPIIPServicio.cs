using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
using System;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.ReportesPIIP
{
    public class ReportePIIPServicio : IReportesPIIPServicio
    {

        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public ReportePIIPServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Servicio que se encarga de ejecutar el reporte seleccionado
        /// </summary>
        /// <returns>datatable con la informacion resultado del reporte</returns>
        public async Task<string> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDNP, string idEntidades, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerDatosReportePIIPMga"];
            //var uriMetodo = ConfigurationManager.AppSettings["urlObtenerDatosReportePIIPMga"] + $"?idReporte={idReporte}" + $"&filtros={filtros}" + $"&idEntidades={idEntidades}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?idReporte={idReporte}" + $"&filtros={filtros}" + $"&idEntidades={idEntidades}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
