namespace DNP.ServiciosNegocio.Dominio.Dto.Catalogos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CatalogoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CatalogoDto> CatalogosRelacionados { get; set; }
    }
}
