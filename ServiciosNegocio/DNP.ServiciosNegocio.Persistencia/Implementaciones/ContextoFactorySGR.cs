using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.

    public class ContextoFactorySGR : IContextoFactorySGR
    {
        public MGAWebContextoSGR CrearContextoSGR(DbConnection conexion)
        {
            return new MGAWebContextoSGR();
        }

        public MGAWebContextoSGR CrearContextoConConexionSGR(string cadenaCompleta)
        {
            return ConstructorDeConexion.CrearContextoProgramaticoSGR(cadenaCompleta);
        }
    }
}
