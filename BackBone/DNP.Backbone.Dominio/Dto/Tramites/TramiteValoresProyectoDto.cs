namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TramiteValoresProyectoDto
    {
        public Nullable<decimal> DecretoNacion { get; set; }
        public Nullable<decimal> DecretoPropios { get; set; }
        public Nullable<decimal> VigenteNacion { get; set; }
        public Nullable<decimal> VigentePropios { get; set; }
        public Nullable<decimal> DisponibleNacion { get; set; }
        public Nullable<decimal> DisponiblePropios { get; set; }
        public Nullable<decimal> VigenciaFuturaNacion { get; set; }
        public Nullable<decimal> VigenciaFuturaPropios { get; set; }
    }
}
