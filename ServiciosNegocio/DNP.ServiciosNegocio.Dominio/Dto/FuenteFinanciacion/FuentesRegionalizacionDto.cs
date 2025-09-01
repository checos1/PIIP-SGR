using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuentesRegionalizacionDto
    {
        public string Agrupacion { get; set; }
        public List<ValoresRegionalizacionDto> ValoresSolicitados { get; set; }
    }
}
