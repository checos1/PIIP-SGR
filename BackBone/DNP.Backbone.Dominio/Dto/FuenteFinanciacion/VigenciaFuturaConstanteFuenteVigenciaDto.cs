using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.FuenteFinanciacion
{
    public class VigenciaFuturaConstanteFuenteVigenciaDto
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public double? Deflactor { get; set; }
        public double? ValorVigenteFutura { get; set; }
        public double? ValorVigenteFuturaCorriente { get; set; }
        public double? ValorVigenteFuturaOriginal { get; set; }
        public double? ValorVigenteFuturaCorrienteOriginal { get; set; }
        public double? ApropiacionVigente { get; set; }
        public bool ValidacionValores { get; set; } = false;
    }
}
