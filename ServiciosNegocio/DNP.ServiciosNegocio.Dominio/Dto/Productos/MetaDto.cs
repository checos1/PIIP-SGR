namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class MetaDto
    {
        public int? MetaId { get; set; }
        public decimal? Meta { get; set; }
        public int? IndicadorId { get; set; }

        public List<RegionalizacionMetasDto> RegionalizacionMetas { get; set; }

    }
}