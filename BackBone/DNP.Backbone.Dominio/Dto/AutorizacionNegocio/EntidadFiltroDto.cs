using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    public class EntidadFiltroDto
    {
        public Guid Id { get; set; }
        public Guid? PadreId { get; set; }
        public string Nombre { get; set; }
        public string PadreSiglas { get; set; }
        public int Nivel { get; set; }

        public Guid? IdAgrupador { get; set; }
        public string AgrupadorEntidad { get; set; }
        public Guid IdEntidad { get; set; }
        public string Entidad { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoEntidad { get; set; }
        public bool CabezaSector { get; set; }
        public bool TieneHijo { get; set; }
        public bool SubEntidad { get; set; }
        public Guid? ParentGuid { get; set; }
        public int EntityType { get; set; }
        public string Sigla { get; set; }
        public int? Codigo { get; set; }
        public Guid? IdSector { get; set; }
        public int SectorNegocioId { get; set; }
        public int? EntityTypeCatalogOptionId { get; set; }
        public IEnumerable<EntidadFiltroDto> SubEntidades { get; set; }
    }
    public class UnidadResponsableDTO
    {
        public Guid UnidadResponsableId { get; set; }

        public int? EntityTypeCatalogOptionId { get; set; }
        public string Nombre { get; set; }

        public int? EntityTypeId { get; set; }

        public int? ParentId { get; set; }

        public Guid? ParentGuid { get; set; }

        public string TipoEntidad { get; set; }

        public Guid? SectorId { get; set; }

        public bool CabezaSector { get; set; }

        public bool IsActivo { get; set; }
        public bool? RolPresupuesto { get; set; }
        public string Sigla { get; set; }
        public int? Codigo { get; set; }
    }

    public class ResultUnidadResponsableDTO
    {
        public List<EntidadTerritorialUnidadesDTO> Resultados { get; set; }
    }

    public class EntidadTerritorialUnidadesDTO
    {
        public Guid IdEntidad { get; set; }
        public string NombreCompleto { get; set; }
        public int? EntityTypeCatalogOptionId { get; set; }
        public bool CabezaSector { get; set; }
        public string TipoEntidad { get; set; }
        public bool HabilitarRegistro { get; set; }
        public System.Collections.Generic.List<UnidadResponsableDTO> UnidadesResponsables { get; set; }
    }

    public class ResultSectorDTO
    {
        public List<SectorDTO> Resultados { get; set; }
    }

    public class SectorDTO
    {
        public Guid Id { get; set; }
        public int SectorNegocioId { get; set; }
        public string Nombre { get; set; }
    }
}
