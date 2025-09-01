using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.FuenteFinanciacion
{
    public class VigenciaFuturaCorrienteDto
    {
        public string BPIN { get; set; }
        public int? ProyectoId { get; set; }
        public int? AñoInicio { get; set; }
        public int? AñoFin { get; set; }
        public int? AnioInicio { get; set; }
        public int? AnioFin { get; set; }
        public double? ValorTotalVigente { get; set; }
        public double? ValorTotalVigenteFutura { get; set; }
        public double? Porcentaje { get; set; }
        public double? ValorPorcentaje { get; set; }
        public bool? cumple { get; set; }
        public List<VigenciaFuturaCorrienteFuenteDto> Fuentes { get; set; }
    }
}
