namespace DNP.ServiciosNegocio.Test.Mock
{
    using DNP.ServiciosNegocio.Persistencia.Interfaces.ReportesPIIP;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class ReportesPIIPPersistenciaMock : IReportesPIIPPersistencia
    {
        private readonly IReportesPIIPPersistencia _ReportesPIIPPersistencia;

        public ReportesPIIPPersistenciaMock(IReportesPIIPPersistencia ireportesPIIPPersistencia)
        {
            _ReportesPIIPPersistencia = ireportesPIIPPersistencia;
        }

        /// <summary>
        /// funcion que se encarga de ejecutar el reporte
        /// </summary>
        /// <returns></returns>
        public DataTable ObtenerDatosReportePIIP(Guid idReporte, string filtros, string idEntidades)
        {
           return _ReportesPIIPPersistencia.ObtenerDatosReportePIIP(idReporte, filtros, idEntidades); ;
        }

    }
}
