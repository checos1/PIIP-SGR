using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class FocalizacionCuantificacionLocalizacionDto
    {
        public string Bpin { get; set; }
        public decimal? PoblacionObjetivo { get; set; }
        public List<FocalizacionCuantificacionDto> Focalizacion { get; set; }
    }
}
