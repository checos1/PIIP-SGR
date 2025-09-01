using DNP.Backbone.Dominio.Dto.Proyecto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion
{
    public class DatosReprogramacionDto
    {
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public List<ValoresReprogramacion> ValoresReprogramacion { get; set; }
    }

    public class ValoresReprogramacion
    {
        public int? Vigencia { get; set; }
        public decimal? ReprogramadoNacion { get; set; }
        public decimal? ReprogramadoPropios { get; set; }
        public bool? ValoresReprogramado { get; set; }
    }
}
