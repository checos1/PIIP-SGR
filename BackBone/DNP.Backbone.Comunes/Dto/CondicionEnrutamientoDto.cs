using System;
using System.Diagnostics.CodeAnalysis;
using DNP.Backbone.Comunes.Enums;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CondicionEnrutamientoDto
    {
        public Guid Id { get; set; }

        public string Campo { get; set; }

        public Condicion Condicion { get; set; }

        public string Valor { get; set; }
        public Guid EnrutamientoId { get; set; }
    }
}
