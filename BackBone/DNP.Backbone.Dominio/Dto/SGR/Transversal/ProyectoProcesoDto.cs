using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR.Transversal
{
    public class ProyectoProcesoDto
    {
        public string BPIN { get; set; }
        public int ProcesoId { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid FlujoId { get; set; }
        public String Usuario { get; set; }
    }

    public class ProyectoProcesoResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
