namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TrazaAccionesPorInstanciaDto
    {
        public Guid InstanciaId { get; set; }
        public Guid AccionId { get; set; }
        public string Observacion { get; set; }
        public string CreadoPor { get; set; }
        public string UltimaObservacion { get; set; }
    }
}
