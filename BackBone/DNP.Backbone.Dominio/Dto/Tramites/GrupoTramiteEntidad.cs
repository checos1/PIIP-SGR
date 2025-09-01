namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class GrupoTramiteEntidad
    {
        public int IdSector { get; set; }
        public string Sector { get; set; }
        public int? EntidadId { get; set; }
        public string NombreEntidad { get; set; }
     
        public List<GrupoTramites> GrupoTramites { get; set; }
    }
}
