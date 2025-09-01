using System;
using System.Diagnostics.CodeAnalysis;


namespace DNP.ServiciosNegocio.Comunes.Excepciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.
    [SerializableAttribute]
    public class ServiciosNegocioException : Exception
    {
        public ServiciosNegocioException()
        {
        }

        public ServiciosNegocioException(string message) : base(message)
        {
        }

        public ServiciosNegocioException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string StackTrace => "";
    }
}
