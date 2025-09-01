using System;

namespace DNP.Backbone.Comunes.Excepciones
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.
    [Serializable]
    public class MotorFlujosException : Exception
    {
        public MotorFlujosException()
        {
        }

        public MotorFlujosException(string message)
            : base(message)
        {
        }

        public MotorFlujosException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string StackTrace => "";
    }
}
