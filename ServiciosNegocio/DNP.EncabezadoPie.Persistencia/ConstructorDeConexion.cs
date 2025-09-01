using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DNP.EncabezadoPie.Persistencia.Modelo;

namespace DNP.EncabezadoPie.Persistencia
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class ConstructorDeConexion
    {
        public static string CrearCadenaDeConexion(string cadenaConexionCompleta)
        {
            const string providerName = "System.Data.SqlClient";
            const string metaDataLocal = "res://{0}/Modelo.ModeloMGAWeb.csdl|res://{0}/Modelo.ModeloMGAWeb.ssdl|res://{0}/Modelo.ModeloMGAWeb.msl";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(cadenaConexionCompleta);

            var metaDataLocalUpdate = string.Format(metaDataLocal, "DNP.EncabezadoPie.Persistencia");
            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder
            {
                Metadata = metaDataLocalUpdate,
                Provider = providerName,
                ProviderConnectionString = sqlBuilder.ConnectionString
            };

            return efBuilder.ConnectionString;
        }


        public static MGAWebContextoEncabezado CrearContextoProgramatico(string cadenaConexionCompleta)
        {
            return new MGAWebContextoEncabezado(CrearCadenaDeConexion(cadenaConexionCompleta));
        }
    }
}
