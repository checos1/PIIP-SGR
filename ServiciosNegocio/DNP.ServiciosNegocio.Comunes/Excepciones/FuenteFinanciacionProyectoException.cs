

namespace DNP.ServiciosNegocio.Comunes.Excepciones
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.
    [SerializableAttribute]
    public class FuenteFinanciacionProyectoException : Exception
    {
        public FuenteFinanciacionProyectoException(string message) : base(message)
        {

        }

        public FuenteFinanciacionProyectoException(string message, Exception inner) : base(message, inner)
        {
        }

        public override string StackTrace => "";

    }
}
