namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TiposEntidadesCatalogoDto : CatalogoDto
    {
        public bool IsBankEntity { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public int? ResourceGroupId { get; set; }
    }
}
