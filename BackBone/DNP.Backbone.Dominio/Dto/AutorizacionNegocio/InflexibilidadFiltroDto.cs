using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    public class InflexibilidadFiltroDto
    {
        public string NombreInflexibilidad { get; set; }
        public double? ValorTotal { get; set; }
        public double? ValorPagado { get; set; }
        public int? AnioInicio { get; set; }
        public int? AnioFin { get; set; }
        public string Estado { get; set; }
    }
}
