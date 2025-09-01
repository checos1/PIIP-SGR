using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar
{
    public class RegionalizacionIndicadorAgregarDto
    {
        public string Bpin { get; set; }
        public List<ObjetivosRegionalizacionIndicadorDto> Objetivos { get; set; }
    }
}
