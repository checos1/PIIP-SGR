using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    public class AvanceMetaProductoDto {

            public int? IndicadorId { get; set; }
            public bool? EsPrincipal { get; set; }
            public int? ProductoId { get; set; }
            public List<PeriodosActivosDto> PeriodosActivos { get; set; }
        }

        public class PeriodosActivosDto
        {
            public int? Vigencia { get; set; }
            public int? PeriodoProyectoId { get; set; }
            public int? PeriodosPeriodicidadId { get; set; }
            public string Observacion { get; set; }
            public double? CantidadEjecutada { get; set; }
        }

}
