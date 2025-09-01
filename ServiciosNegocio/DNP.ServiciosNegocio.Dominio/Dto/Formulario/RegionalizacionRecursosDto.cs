using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class RegionalizacionRecursosDto
    {
        public int? RegionalizacionRecursosId  { get; set; }
        public int? RegRecursoRegionID { get; set; }

        public string RegRecursoRegion { get; set; }

        public int? RegRecursosDepartamentoId { get; set; }
        public string RegRecursosDepartamento { get; set; }

        public int? RegRecursosMunicipioId { get; set; }
        public string RegRecursosMunicipio { get; set; }

        public int? RegRecursosAgrupacionId { get; set; }
        public string RegRecursosAgrupacion { get; set; }
        public string RegRecursosRegionalizacionNombre { get; set; }

        public decimal? RegValorSolicitado { get; set; }

        public decimal? RegValorInicial { get; set; }
        public decimal? RegValorVigente { get; set; }
        public decimal? RegCompromiso { get; set; }
        public decimal? RegObligacion { get; set; }
        public decimal? RegPago { get; set; }
        public int? EjecucionRecursosId { get; set; }
    }
}
