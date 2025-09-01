namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class FocalizacionCuantificacionDto
    {
        public int? PoliticaId { get; set; }
        public int? DimensionId { get; set; }
        public string NombreDescripcion { get; set; }
        public decimal? CantidadMGA { get; set; }
        public List<VigenciasFocalizacionCuantificacionDto> Vigencias { get; set; }
    }
}
