using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosTransaccional.Persistencia.Interfaces;
using DNP.ServiciosTransaccional.Persistencia.Modelo;

namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class ContextoFactory : IContextoFactory
    {
        public MGAWebContextoTransaccional CrearContexto(DbConnection conexion)
        {
            return new MGAWebContextoTransaccional();
        }

        public MGAWebContextoTransaccional CrearContextoConConexion(string cadenaCompleta)
        {
            return ConstructorDeConexion.CrearContextoProgramatico(cadenaCompleta);
        }
    }
}
