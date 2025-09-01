using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar
{
    public class ProductosRegionalizacionIndicadorDto
    {
        public int? ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoProducto { get; set; }
        public int? ProductoUnidadMedidaId { get; set; }
        public string ProductoUnidadMedida { get; set; }
        public decimal? Cantidad { get; set; }
        public List<IndicadoresRegionalizacionIndicadorDto> Indicadores { get; set; }
    }
}
