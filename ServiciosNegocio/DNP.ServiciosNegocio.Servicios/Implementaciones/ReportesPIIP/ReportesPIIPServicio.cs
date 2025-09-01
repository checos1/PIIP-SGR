

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.ReportesPIIP
{
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.ReportesPIIP;
    using DNP.ServiciosNegocio.Servicios.Interfaces.ReportesPIIP;

    public class ReportesPIIPServicio : IReportesPIIPServicio
    {
        private readonly IReportesPIIPPersistencia _objReportesPIIPPersistencia;
        private readonly ICacheServicio _cacheServicio;

        public ReportesPIIPServicio(ICacheServicio cacheServicio, IReportesPIIPPersistencia reportesPIIPPersistencia)
        {
            _cacheServicio = cacheServicio;
            _objReportesPIIPPersistencia = reportesPIIPPersistencia;

        }

        /// <summary>
        /// funcion que se encarga de ejecutar el reporte
        /// </summary>
        /// <returns></returns>
        public DataTable ObtenerDatosReportePIIP(Guid idReporte, string filtros, string idEntidades)
        {
           return _objReportesPIIPPersistencia.ObtenerDatosReportePIIP(idReporte, filtros, idEntidades);
        }
    }
}
