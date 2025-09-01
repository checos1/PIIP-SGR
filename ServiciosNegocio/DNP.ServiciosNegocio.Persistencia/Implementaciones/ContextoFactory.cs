using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class ContextoFactory : IContextoFactory
    {
        public MGAWebContexto CrearContexto(DbConnection conexion)
        {
            return new MGAWebContexto();
        }

        public MGAWebContexto CrearContextoConConexion(string cadenaCompleta)
        {
            return ConstructorDeConexion.CrearContextoProgramatico(cadenaCompleta);
        }

        public ModelOnlySPEntities CrearContextoOnlySP(DbConnection conexion)
        {
            return new ModelOnlySPEntities();
        }

        public ModelOnlySPEntities CrearContextoConConexionOnlySP(string cadenaCompleta)
        {
            return ConstructorDeConexion.CrearContextoProgramaticoOnlySP(cadenaCompleta);
        }
    }
}
