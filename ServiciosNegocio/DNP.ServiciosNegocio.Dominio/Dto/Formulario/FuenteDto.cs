using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class FuenteDto
    {
        public int? FuenteId{ get; set; }
        public string GrupoRecurso{ get; set; }
        public string Nombre { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ValorInicial { get; set; }
        public decimal? ValorVigente { get; set; }
        public decimal? Compromiso { get; set; }
        public decimal? Obligacion { get; set; }
        public decimal? Pago { get; set; }
        public List<RegionalizacionRecursosDto> Regionalizacion { get; set; }

    }
}
