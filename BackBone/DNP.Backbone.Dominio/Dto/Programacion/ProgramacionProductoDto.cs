using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProgramacionProductoDto
    {
        public int TramiteId { get; set; }
        public string NivelId { get; set; }
        public int SeccionCapitulo { get; set; }
        public List<ProgramacionProductos> ProgramacionProductos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProgramacionProductos
    {
        public int ProductCatalogId { get; set; }
        public decimal? Meta { get; set; }
        public decimal? Recurso { get; set; }
    }
}