namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EntidadCatalogoDto : CatalogoDto
    {
        public int? EntityTypeId { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EntidadCatalogoDTDto : CatalogoDto
    {
        public int? EntityTypeCatalogOptionId { get; set; }
        public int? EntityTypeId { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EntidadCatalogoSTDto : CatalogoDto
    {
        public int? EntityTypeCatalogOptionId { get; set; }
        public int? EntityTypeId { get; set; }
        public int? DireccionTecnicaId { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
    }
}
