using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EntidadNegocioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string TipoEntidad { get; set; }
        public SectorNegocioDto Sector { get; set; }
        public bool CabezaSector { get; set; }
        public Guid? ParentGuid { get; set; }
        public int EntityType { get; set; }
        public string Sigla { get; set; }
        public int? Codigo { get; set; }
        public Guid? IdSector { get; set; }
        public bool IsActivo { get; set; }
        public bool SubEntidad { get; set; }
        public int? EntityTypeCatalogOptionId { get; set; }
        public int? ParentId { get; set; }
        public bool? RolPresupuesto { get; set; }
    }
}

