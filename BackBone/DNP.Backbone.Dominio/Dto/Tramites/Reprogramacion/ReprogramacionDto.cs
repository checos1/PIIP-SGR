using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion
{
    public class ReprogramacionDto
    {
        public int? Id { get; set; }
        public int? TramiteProyectoId { get; set; }
        public int? AutorizacionVigenciasFuturasId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
    }
}
