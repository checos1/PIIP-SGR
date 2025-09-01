using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario
{
    [ExcludeFromCodeCoverage]
    public class PoblacionVigenciasDto
    {
        public int? Vigencia { get; set; }
        public decimal? TotalBeneficiariosProyecto { get; set; }
        public List<PoblacionVigenciaLocalizacionDto> Localizacion { get; set; }
    }
}
