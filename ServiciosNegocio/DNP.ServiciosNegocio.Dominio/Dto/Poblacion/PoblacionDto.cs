using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Poblacion
{

    [ExcludeFromCodeCoverage]
    public class PoblacionDto
    {
        public string Bpin { get; set; }
        public decimal? CantidadPoblacion { get; set; }
        public List<PoblacionVigenciasDto> Vigencias { get; set; }
    }
}
