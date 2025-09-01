using DNP.ServiciosEnrutamiento.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Persistencia
{
    [ExcludeFromCodeCoverage]
    public class ConstructorDeConexion
    {
        public static string CrearCadenaDeConexion(string cadenaConexionCompleta)
        {
            const string providerName = "System.Data.SqlClient";
            const string metaDataLocal = "res://*/Modelo.ModeloMGAWeb.csdl|res://*/Modelo.ModeloMGAWeb.ssdl|res://*/Modelo.ModeloMGAWeb.msl";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(cadenaConexionCompleta);


            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder
            {
                Metadata = metaDataLocal,
                Provider = providerName,
                ProviderConnectionString = sqlBuilder.ConnectionString
            };

            return efBuilder.ConnectionString;
        }


        public static MGAWebContexto CrearContextoProgramatico(string cadenaConexionCompleta)
        {
            return new MGAWebContexto(CrearCadenaDeConexion(cadenaConexionCompleta));
        }
    }
}
