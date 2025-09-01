using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar
{
    public class ObjetivosRegionalizacionIndicadorDto
    {
        public int? ObjetivoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductosRegionalizacionIndicadorDto> Productos { get; set; }
    }
}
