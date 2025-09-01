namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DetalleEjecucionDto
    {
        public int Mes { get; set; }
        public double? ValorInicial { get; set; }
        public double? ValorVigente { get; set; }
        public double? Compromiso { get; set; }
        public double? Obligacion { get; set; }
        public double? Pago { get; set; }
    }
}
