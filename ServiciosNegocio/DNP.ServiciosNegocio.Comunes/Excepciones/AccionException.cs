using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Comunes.Excepciones
{
    [ExcludeFromCodeCoverage]
    //Se excluye por ser extensión de una clase del Framework. No se hace extensión de los métodos, sirve para identificar el tipo de excepción en el sistema.

    [Serializable]
    public class AccionException : Exception
    {
        public AccionException()
        {
        }

        public AccionException(string message) : base(message)
        {
        }

        public AccionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string StackTrace => "";
    }
}
