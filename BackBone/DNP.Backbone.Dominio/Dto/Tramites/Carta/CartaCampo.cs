namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CartaCampo
    {
        public int Id { get; set; }
        public int CartaConceptoSeccionId { get; set; }
        public int PlantillaCartaCampoId { get; set; }
        public string DatoValor { get; set; }
        public string NombreCampo { get; set; }
        public TipoCampo TipoCampo { get; set; }
    }

}
