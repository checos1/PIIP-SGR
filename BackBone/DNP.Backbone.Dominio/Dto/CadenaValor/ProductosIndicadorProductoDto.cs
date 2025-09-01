using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class ProductosIndicadorProductoDto
    {
        public int? ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string LabelBotonProducto { get; set; }
        public List<IndicadoresIndicadorProductoDto> Indicadores { get; set; }
        public List<IndicadoresIndicadorSecundarioProductoDto> CatalogoIndicadoresSecundarios { get; set; }

    }
}
