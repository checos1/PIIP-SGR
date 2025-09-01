using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class LocalizacionFirmeDto
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
        public int? Enfirme { get; set; }

    }
}
