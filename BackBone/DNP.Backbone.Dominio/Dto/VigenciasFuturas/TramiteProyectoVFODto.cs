using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.VigenciasFuturas
{
    public class TramiteProyectoVFODto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public string NombreProyecto { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public string Accion { get; set; }
        public string TipoProyecto { get; set; }
        public int? TramiteId { get; set; }
        public int? EntidadId { get; set; }
        public string NombreEntidad { get; set; }
    }
}
