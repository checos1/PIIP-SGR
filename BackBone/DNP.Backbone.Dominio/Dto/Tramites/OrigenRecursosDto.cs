using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class OrigenRecursosDto
    {
        public int TramiteId { get; set; }
        public int TipoOrigenId { get; set; }
        public string Rubro { get; set; }
    }
}
