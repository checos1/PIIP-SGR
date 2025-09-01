using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario
{
    [ExcludeFromCodeCoverage]
    public class PoblacionDto
    {
        public string BPIN { get; set; }
        public int? PoblacionAfectada { get; set; }
        public int? PoblacionObjetivo { get; set; }
        public decimal? ValorTotalProyecto { get; set; }
        public List<PoblacionVigenciasDto> Vigencias { get; set; }
    }
}
