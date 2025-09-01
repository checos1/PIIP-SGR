using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class RegionalizacionDto
    {
        public int ProjectId { get; set; }
        public string Bpin { get; set; }
        public double ValorTotalSolicitadoProyecto { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public string FuenteDescripcion { get; set; }
        public int FuenteEtapaId { get; set; }
        public int FfProgramacionId { get; set; }
        public int FfProgramacionVigencia { get; set; }
        public double FfProgramacionValorSolicitado { get; set; }
        public int? RegRecursosId { get; set; }
        public int? RegRecursosRegionId { get; set; }
        public int? RegRecursosDepartamentoId { get; set; }
        public int? RegRecursosMunicipioId { get; set; }
        public int? RegRecursosAgrupacionId { get; set; }
        public int FuenteId { get; set; }
        public double? ValorVigente { get; set; }
        public double? ValorInicial { get; set; }
        public double? ValorSocilitado { get; set; }
        public int? Mes { get; set; }
        public double? EjecucionValorInicial { get; set; }
        public double? EjecucionValorVigente { get; set; }
        public double? EjecucionCompromiso { get; set; }
        public double? EjecucionObligacion { get; set; }
        public double? EjecucionPago { get; set; }
    }
}
