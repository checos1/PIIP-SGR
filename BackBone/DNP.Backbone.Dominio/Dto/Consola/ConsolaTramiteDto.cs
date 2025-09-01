namespace DNP.Backbone.Dominio.Dto.Consola
{
    using DNP.Backbone.Dominio.Dto.Tramites;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ConsolaTramiteDto
    {
        public string Mensaje { get; set; }
        public string[] ColumnasVisibles { get; set; }
        public List<GrupoTramiteEntidad> ListaGrupoTramiteEntidad { get; set; }
    }
}
