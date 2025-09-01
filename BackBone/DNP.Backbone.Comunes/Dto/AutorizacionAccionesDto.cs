using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class AutorizacionAccionesDto
    {
        public Guid Id { get; set; }
        public Guid RolId { get; set; }
        public int IdEntidad { get; set; }
        public string Entidad { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
