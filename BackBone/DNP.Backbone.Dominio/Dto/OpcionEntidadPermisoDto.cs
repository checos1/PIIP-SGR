using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    public class OpcionEntidadPermisoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        public Guid? IdOpcionPadre { get; set; }
        public string OpcionPadre { get; set; }

        public string IdOpcionDNP { get; set; }
        public string Tipo { get; set; }
    }
}
