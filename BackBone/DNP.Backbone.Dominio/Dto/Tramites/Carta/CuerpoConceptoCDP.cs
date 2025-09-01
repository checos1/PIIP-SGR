namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CuerpoConceptoCDP
    {        public string CDP { get; set; }
        public DateTime FechaCDP { get; set; }
        public decimal ValorCDP { get; set; }
    }
}
