using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuenteFinanciacionRegionalizacionDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int? CR { get; set; }
        public List<FuentesRegionalizacionDto> Fuentes { get; set; }
        public List<RegionalizacionDto> Regionalizacion { get; set; }
    }
}
