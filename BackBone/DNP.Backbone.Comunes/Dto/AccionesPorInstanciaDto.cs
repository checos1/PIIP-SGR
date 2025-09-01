using System;

namespace DNP.Backbone.Comunes.Dto
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class AccionesPorInstanciaDto
    {
        public Guid Id { get; set; }
        public int EstadoAccionPorInstanciaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public Guid InstanciaId { get; set; }
        public Guid AccionId { get; set; }
        public int? Enrutamiento { get; set; }
    }
}
