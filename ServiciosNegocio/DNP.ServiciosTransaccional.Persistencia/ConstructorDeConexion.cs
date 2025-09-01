using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosTransaccional.Persistencia.Modelo;
using DNP.ServiciosTransaccional.Persistencia.ModeloSGR;

namespace DNP.ServiciosTransaccional.Persistencia
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class ConstructorDeConexion
    {
        public static string CrearCadenaDeConexion(string cadenaConexionCompleta)
        {
            const string providerName = "System.Data.SqlClient";
            const string metaDataLocal = "res://*/Modelo.MGAWebContextoTransaccional.csdl|res://*/Modelo.MGAWebContextoTransaccional.ssdl|res://*/Modelo.MGAWebContextoTransaccional.msl";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(cadenaConexionCompleta);


            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder
            {
                Metadata = metaDataLocal,
                Provider = providerName,
                ProviderConnectionString = sqlBuilder.ConnectionString
            };

            return efBuilder.ConnectionString;
        }

        public static string CrearCadenaDeConexionSGR(string cadenaConexionCompleta)
        {
            const string providerName = "System.Data.SqlClient";
            const string metaDataLocal = "res://*/ModeloSGR.ModeloMGAWebSGR.csdl|res://*/ModeloSGR.ModeloMGAWebSGR.ssdl|res://*/ModeloSGR.ModeloMGAWebSGR.msl";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(cadenaConexionCompleta);


            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder
            {
                Metadata = metaDataLocal,
                Provider = providerName,
                ProviderConnectionString = sqlBuilder.ConnectionString
            };

            return efBuilder.ConnectionString;
        }

        public static MGAWebContextoTransaccional CrearContextoProgramatico(string cadenaConexionCompleta)
        {
            return new MGAWebContextoTransaccional(CrearCadenaDeConexion(cadenaConexionCompleta));
        }

        public static MGAWebContextoSGR CrearContextoProgramaticoSGR(string cadenaConexionCompleta)
        {
            return new MGAWebContextoSGR(CrearCadenaDeConexionSGR(cadenaConexionCompleta));
        }
    }
}
