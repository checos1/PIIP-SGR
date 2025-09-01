using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class RegionalizacionDto
    {
        public int? FuenteId { get; set; }
        public int? ProgramacionFuenteId { get; set; }
        public string FuenteFinanciacion { get; set; }
        public int? Vigencia { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public List<LocalizacionRegionalizacionDto> Localizacion { get; set; }
    }
}