using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    public class TrazaAccionDto
    {
        public Guid? AccionFlujoId { get; set; }
        public string AccionesFlujos { get; set; }
        public Guid? TrazaId { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string NombreUsuario { get; set; }


    }
}
