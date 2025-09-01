namespace DNP.Backbone.Servicios.Interfaces.ReportePIIP
{
    using System;
    using System.Data;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public interface IReportesPIIPServicio
    {
        /// <summary>
        /// Servicio que se encarga de ejecutar el reporte seleccionado
        /// </summary>
        /// <returns>datatable con la informacion resultado del reporte</returns>
        Task<string> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDNP, string idEntidades, string tokenAutorizacion);

    }
}
