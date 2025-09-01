namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    using System.Collections.Generic;

    public class VigenciaFuturaConstanteDto
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
        public List<VigenciaFuturaConstanteFuenteDto> Fuentes { get; set; }
    }
}
