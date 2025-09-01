using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    public class PeriodoPresidencialDto
    {
        public int AnoInicial { get; set; } 

        public int AnoFinal { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }
    }
}
