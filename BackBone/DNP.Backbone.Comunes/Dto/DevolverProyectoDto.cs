namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DevolverProyectoDto
    {
        public Guid InstanciaId { get; set; }
        public string Bpin { get; set; }
        public int? ProyectoId { get; set; }
        public string Observacion { get; set; }
        public bool? DevolverId { get; set; }
        public int? EstadoDevolver { get; set; }
    }
}
