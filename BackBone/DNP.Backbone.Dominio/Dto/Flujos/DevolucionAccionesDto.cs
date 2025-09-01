using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    public class DevolucionAccionesDto
    {
        public Guid IdAccionPrincipal { get; set; }

        public Guid IdAccionDevolucion { get; set; }

        public string NombreAccionDevolucion { get; set; }
    }
}
