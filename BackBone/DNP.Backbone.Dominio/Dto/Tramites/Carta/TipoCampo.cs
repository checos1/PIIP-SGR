namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TipoCampo
    {
        public int TipoCampoId { get; set; }
        public int Longitud { get; set; }
        public string Formato { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

}
