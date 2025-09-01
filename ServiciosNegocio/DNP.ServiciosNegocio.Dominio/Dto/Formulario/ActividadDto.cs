using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    [ExcludeFromCodeCoverage]
    public class ActividadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? IdActividadPorInsumo { get; set; }
        public int? IdInsumo { get; set; }
        public string NombreInsumo { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ApropiacionInicial { get; set; }
        public decimal? ApropiacionVigente { get; set; }
        public List<EjecucionDto> Ejecuciones { get; set; }
    }
}