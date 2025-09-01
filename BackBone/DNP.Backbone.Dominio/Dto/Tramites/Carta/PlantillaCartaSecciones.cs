namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PlantillaCartaSecciones
    {
        public int Id { get; set; }
        public int PlantillaCartaId { get; set; }
        public string NombreSeccion { get; set; }
        public List<Campo> PlantillaSeccionCampos { get; set; }
    }

}
