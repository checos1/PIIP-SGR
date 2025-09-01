namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TramitesProyectosDto
    {
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public Guid? InstanciaId { get; set; }
        public int EntidadId { get; set; }
    }
}
