namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class Campo
    {
        public int Id { get; set; }
        public int PlantillaCartaSeccionId { get; set; }
        public string NombreCampo { get; set; }
        public TipoCampo TipoCampo { get; set; }
        public string TituloCampo { get; set; }
        public string TextoDefecto { get; set; }
        public bool Editable { get; set; }
        public int Orden { get; set; }
        public string ConsultaSql { get; set; }
        public string ValorCampo { get; set; }
    }

}
