namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public class ReportesPIIPServicioMock : IReportesPIIPServicio
    {
        IReportesPIIPServicio _IReportesPIIPServicio;

        public ReportesPIIPServicioMock(IReportesPIIPServicio ReportesPIIPServicio)
        {
            _IReportesPIIPServicio = ReportesPIIPServicio;
        }

        public Task<string> ObtenerDatosReportePIIP(Guid idReporte, string filtros, string usuarioDNP, string idEntidades, string tokenAutorizacion)
        {
            return _IReportesPIIPServicio.ObtenerDatosReportePIIP(idReporte, filtros, usuarioDNP, idEntidades, tokenAutorizacion);
        }

    }
}