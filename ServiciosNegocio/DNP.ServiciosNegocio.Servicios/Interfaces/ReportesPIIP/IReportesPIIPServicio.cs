
namespace DNP.ServiciosNegocio.Servicios.Interfaces.ReportesPIIP
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IReportesPIIPServicio
    {
        /// <summary>
        /// funcion que se encarga de ejecutar el reporte
        /// </summary>
        /// <returns></returns>
        DataTable ObtenerDatosReportePIIP(Guid idReporte, string filtros, string idEntidades);
    }
}
