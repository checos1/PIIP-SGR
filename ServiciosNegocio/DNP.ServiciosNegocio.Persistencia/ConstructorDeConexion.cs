using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;

namespace DNP.ServiciosNegocio.Persistencia
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

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


        public static MGAWebContexto CrearContextoProgramatico(string cadenaConexionCompleta)
        {
            return new MGAWebContexto(CrearCadenaDeConexion(cadenaConexionCompleta));
        }

        public static string CrearCadenaDeConexionOnlySP(string cadenaConexionCompleta) 
        {
            const string providerName = "System.Data.SqlClient";
            const string metaDataLocal = "res://*/Modelo_OnlySP.ModelOnlySP.csdl|res://*/Modelo_OnlySP.ModelOnlySP.ssdl|res://*/Modelo_OnlySP.ModelOnlySP.msl";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(cadenaConexionCompleta);


            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder
            {
                Metadata = metaDataLocal,
                Provider = providerName,
                ProviderConnectionString = sqlBuilder.ConnectionString
            };

            return efBuilder.ConnectionString;
        }
        public static ModelOnlySPEntities CrearContextoProgramaticoOnlySP(string cadenaConexionCompleta)
        {
            return new ModelOnlySPEntities(CrearCadenaDeConexionOnlySP(cadenaConexionCompleta));
        }




        public static MGAWebContextoSGR CrearContextoProgramaticoSGR(string cadenaConexionCompleta)
        {
            return new MGAWebContextoSGR(CrearCadenaDeConexionSGR(cadenaConexionCompleta));
        }
    }
}
