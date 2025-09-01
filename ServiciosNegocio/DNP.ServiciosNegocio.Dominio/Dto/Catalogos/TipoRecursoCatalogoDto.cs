namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TipoRecursoCatalogoDto:CatalogoDto
    {
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
        public int? EntityTypeId { get; set; }
        public string Code { get; set; }
        public decimal? IdEscritorio { get; set; }
        public decimal? IdClassificationType { get; set; }
        public int? ResourceGroupId { get; set; }
        
    }
}
