using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Excepciones
{

    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.
    [Serializable]
    public class BackboneException : Exception
    {
        public IEnumerable<string> Erros { get; set; }
        public bool Resultado { get; set; }

        public BackboneException(bool resultado, IEnumerable<string> erros)
        {
            Resultado = resultado;
            Erros = erros;
        }

        public BackboneException()
        {
        }

        public BackboneException(string message)
            : base(message)
        {
        }

        public BackboneException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string StackTrace => "";
    }
}