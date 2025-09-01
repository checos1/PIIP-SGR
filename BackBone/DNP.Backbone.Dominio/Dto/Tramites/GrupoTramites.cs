namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class GrupoTramites
    {
        public int? TipoTramiteId { get; set; }
        public string NombreTipoTramite { get; set; }
        public int? SectorId { get; set; }
        public string NombreSector { get; set; }
        public int? EntidadId { get; set; }
        public string NombreEntidad { get; set; }
        public int? TipoEntidadId { get; set; }
        public string NombreTipoEntidad { get; set; }
        public string NombreSectorTramite { get; set; }
        public string DescripcionTramite { get; set; }
        public string NombreFlujo { get; set; }
        public DateTime? FechaCreacionTramite { get; set; }
        public List<TramiteDto> ListaTramites { get; set; }
    }
}
