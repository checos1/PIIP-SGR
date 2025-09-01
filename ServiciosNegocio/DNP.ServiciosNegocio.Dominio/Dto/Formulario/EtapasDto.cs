using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EtapaDto
    {
        public int Id { get; set; }
        public string NombreEtapa { get; set; }
        public decimal? TotalEtapaValorSolicitado { get; set; }
        public decimal? TotalEtapaApropiacionInicial { get; set; }
        public decimal? TotalEtapaApropiacionVigente { get; set; }
        public List<ActividadDto> Actividades { get; set; }
    }
}