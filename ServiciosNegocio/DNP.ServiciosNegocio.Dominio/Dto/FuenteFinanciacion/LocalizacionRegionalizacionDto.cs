namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class LocalizacionRegionalizacionDto
    {
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public int? AgrupacionId { get; set; }
        public string Agrupacion { get; set; }
        public int? RegionalizacionRecursosId { get; set; }
        public decimal? Valor { get; set; }
    }
}
