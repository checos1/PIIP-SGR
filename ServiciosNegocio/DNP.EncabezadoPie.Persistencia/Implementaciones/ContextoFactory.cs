using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DNP.EncabezadoPie.Persistencia.Interfaces;
using DNP.EncabezadoPie.Persistencia.Modelo;

namespace DNP.EncabezadoPie.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class ContextoFactory : IContextoFactory
    {
        public MGAWebContextoEncabezado CrearContexto(DbConnection conexion)
        {
            return new MGAWebContextoEncabezado();
        }

        public MGAWebContextoEncabezado CrearContextoConConexion(string cadenaCompleta)
        {
            return ConstructorDeConexion.CrearContextoProgramatico(cadenaCompleta);
        }
    }
}
