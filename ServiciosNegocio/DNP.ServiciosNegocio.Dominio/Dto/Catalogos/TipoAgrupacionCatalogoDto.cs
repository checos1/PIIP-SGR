namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TipoAgrupacionCatalogoDto:CatalogoDto
    {
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        
       
    }
}
