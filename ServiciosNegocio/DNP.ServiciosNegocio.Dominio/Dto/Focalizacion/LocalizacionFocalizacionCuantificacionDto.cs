namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class LocalizacionFocalizacionCuantificacionDto
    {
        public int? LocalizacionId { get; set; }
        public int? RegionId { get; set; }
        public string NombreRegion { get; set; }
        public string CodigoRegion { get; set; }
        public int? DepartamentoId { get; set; }
        public string NombreDepartamento { get; set; }
        public string CodigoDepto { get; set; }
        public int? MunicipioId { get; set; }
        public string NombreMunicipio { get; set; }
        public string CodigoMunicipio { get; set; }
        public int? AgrupacionId { get; set; }
        public string NombreAgrupacion { get; set; }
        public string CodigoAgrupacion { get; set; }
        public decimal? CantidadCuantificada { get; set; }
        public decimal? CantidadFocalizada { get; set; }
    }
}
