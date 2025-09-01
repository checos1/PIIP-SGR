using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class CalendarioPeriodoDto
    {
        public int Id { get; set; }
        public int? FaseId { get; set; }
        public int PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public string Mes { get; set; }
        public bool Activo { get; set; }
    }
}
