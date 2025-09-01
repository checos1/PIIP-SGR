namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class RegionalizacionMetasDto
    {
        public int? RegionalizacionMetaId { get; set; }
        public int? RegionalizacionMetaRegionId { get; set; }
        public string Region { get; set; }
        public int? RegionalizacionMetaDepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? RegionalizacionMetaMunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? RegionalizacionMetaAgrupacionId { get; set; }
        public string Agrupacion { get; set; }
        public decimal? RegionalizacionMetaVigente { get; set; }
        public decimal? RegionalizacionMetaTotalRegionalizado { get; set; }
        public int? MetaId { get; set; }
        public decimal? RegionalizacionMetaRezagada { get; set; }
        public decimal? RegionalizacionMetaTotalRezagada { get; set; }
        public decimal? RegionalizacionMetaAvance { get; set; }
        public decimal? RegionalizacionMetaTotalAvance { get; set; }
        public decimal? RegionalizacionMetaAvanceRezago { get; set; }
        public decimal? RegionalizacionMetaTotalAvanceRezago { get; set; }
        public decimal? RegionalizacionMetaAvanceAcumulado { get; set; }
        public decimal? RegionalizacionMetaTotalAvanceAcumulado { get; set; }

    }
}
