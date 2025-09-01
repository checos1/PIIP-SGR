using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Conpes
{
    public class DocumentoCONPES
    {
        public int id { get; set; }
        public string ano { get; set; }
        public string titulo { get; set; }
        public string numeroCONPES { get; set; }
        public DateTime? fechaAprobacion { get; set; }
        public string tipoCONPES { get; set; }
        public string docNombre { get; set; }
        public string docUrl { get; set; }
        public int seleccionado { get; set; }
        public int proyectoId { get; set; }

    }

    public class CapituloConpes
    {
        public List<DocumentoCONPES> Conpes { get; set; }
        public int ProyectoId { get; set; }
        public string Justificacion { get; set; }
        public string GuiMacroproceso { get; set; }
        public int Capitulo { get; set; }
        public int SeccionCapituloId { get; set; }
        public Guid InstanciaId { get; set; }
    }

}
