using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar
{
    public class VigenciasRegionalizacionIndicadorDto
    {
        public int? Vigencia { get; set; }
        public decimal? MetaVigencia { get; set; }
        public List<RegionalizacionRegionalizacionIndicadorDto> Regionalizacion { get; set; }
    }
}
