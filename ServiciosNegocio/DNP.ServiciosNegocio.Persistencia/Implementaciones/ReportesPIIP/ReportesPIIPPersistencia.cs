namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.ReportesPIIP
{
    using AutoMapper;
    using DNP.ServiciosNegocio.Persistencia.Interfaces;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.ReportesPIIP;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;


    public class ReportesPIIPPersistencia : Persistencia, IReportesPIIPPersistencia
    {
        public ReportesPIIPPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }


        /// <summary>
        /// funcion que se encarga de ejecutar el reporte
        /// </summary>
        /// <returns></returns>
        public DataTable ObtenerDatosReportePIIP(Guid idReporte, string filtros, string idEntidades)
        {
            try
            {
                //string listEntidades = string.Empty;

                //if (idEntidades.ToList().Count > 0)
                //{
                //    idEntidades.ToList().ForEach(x => { listEntidades += x + ","; });

                //    listEntidades = listEntidades.Remove(listEntidades.Length - 1, 1);
                //}

                DataSet dsReportes = new DataSet();
                DataTable ds = new DataTable();

                if (filtros == "null") filtros = string.Empty;
                DataTable table = new DataTable();

                using (var con = new SqlConnection(Contexto.Database.Connection.ConnectionString))
                using (var cmd = new SqlCommand("Reportes.spGetDatosReportePIIP", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReporte", idReporte);
                    cmd.Parameters.AddWithValue("@filtros", filtros);
                    cmd.Parameters.AddWithValue("@idEntidades", idEntidades);
                    da.Fill(table);
                }

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


