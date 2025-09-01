namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ProgramaCatalogoDto : CatalogoDto
    {
        public int IdSector { get; set; }
        public string BankCode { get; set; }
        public bool? IsTerritorial { get; set; }
        public bool IsActive { get; set; }
    }
}
