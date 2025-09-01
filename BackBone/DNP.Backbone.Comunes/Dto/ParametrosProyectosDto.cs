namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosProyectosDto
    {
        public List<int> IdsEntidades { get; set; }
        public List<string> NombresEstadosProyectos { get; set; }
        public string TokenAutorizacion { get; set; }
        public int? IdTramite { get; set; }
        public string IdUsuarioDNP { get; set; }
    }
}
