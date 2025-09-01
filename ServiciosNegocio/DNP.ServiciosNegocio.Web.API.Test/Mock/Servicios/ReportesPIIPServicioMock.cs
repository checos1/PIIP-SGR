namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.ReportesPIIP;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    public class ReportesPIIPServicioMock : IReportesPIIPServicio
    {
        public DataTable ObtenerDatosReportePIIP(Guid idReporte, string filtros, string idEntidades)
        {
            DataTable dt = new DataTable();
            return dt;
        }

    }
}

