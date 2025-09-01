using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class EjecucionMesDto
    {
        public int? Mes { get; set; }
        public string NombreMes { get; set; }
        public decimal? ValorInicial { get; set; }
        public decimal? ValorVigente { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }

    }
}
