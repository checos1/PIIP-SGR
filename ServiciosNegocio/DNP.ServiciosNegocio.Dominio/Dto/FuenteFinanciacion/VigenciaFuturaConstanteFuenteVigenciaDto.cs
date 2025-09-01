using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
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
