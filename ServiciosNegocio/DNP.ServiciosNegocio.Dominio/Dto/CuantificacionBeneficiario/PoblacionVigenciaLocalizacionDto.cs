using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario
{
    [ExcludeFromCodeCoverage]
    public class PoblacionVigenciaLocalizacionDto
    {
        public int? LocalizacionId { get; set; }
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? AgrupacionId { get; set; }
        public string NombreAgrupacion { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public decimal? NumeroBeneficiarios { get; set; }
    }
}
