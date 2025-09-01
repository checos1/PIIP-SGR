using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class ProgramacionExcepcionDto
    {
        public int IdProgramacionExcepcion { get; set; }

        public int IdProgramacion { get; set; }

        public int EntidadId { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }
        public ProgramacionDto Programacion { get; set; }

    }
}
