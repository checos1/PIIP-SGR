using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class CatalogoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CatalogoDto> CatalogosRelacionados { get; set; }
    }
}
