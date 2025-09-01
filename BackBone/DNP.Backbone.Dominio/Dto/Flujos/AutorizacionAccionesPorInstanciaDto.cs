using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    public class AutorizacionAccionesPorInstanciaDto
    {
        public Guid InstanciaId { get; set; }
        public Guid RolId { get; set; }
        public Guid FlujoId { get; set; }

    }
}
