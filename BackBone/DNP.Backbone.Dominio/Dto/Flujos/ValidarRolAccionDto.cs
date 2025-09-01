using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    public class ValidarRolAccionDto
    {
        public List<Guid> Roles { get; set; }
        public Guid accionId { get; set; }
        public Guid InstanciaId { get; set; }

    }
}
