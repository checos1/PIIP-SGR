using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    public class CatalogoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CatalogoDto> CatalogosRelacionados { get; set; }
    }


}
