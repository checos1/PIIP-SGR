namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ProductoCatalogoDto : CatalogoDto
    {
        public int ProgramId { get; set; }
        public string Description { get; set; }
        public int MeasureTypeId { get; set; }
        public string Codigo { get; set; }
        public bool? IsTerritorial { get; set; }
        public bool IsActive { get; set; }
    }
}
