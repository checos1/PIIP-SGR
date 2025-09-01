using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class ProyectoPriorizarDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public int EntidadId { get; set; }
        public string NombreEntidad { get; set; }
        public bool Priorizado { get; set; }
        public int Orden { get; set; }
        public bool PermitePriorizar { get; set; }
        public Guid FlujoId { get; set; }
    }
}
