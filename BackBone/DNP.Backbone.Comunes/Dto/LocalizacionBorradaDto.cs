using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class LocalizacionBorradaDto
    {
        public int? id { get; set; }
        public int? DepartamentId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipalityId { get; set; }
        public string Municipio { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public int? AgrupacionId { get; set; }
        public string Agrupacion { get; set; }
        public int? Borrados { get; set; }
    }
}
