using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class EntidadCatalogoDTDto : CatalogoDto
    {
        public int? EntityTypeCatalogOptionId { get; set; }
        public int? EntityTypeId { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
    }

}
