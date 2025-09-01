using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    public class ProductosConstantesVF
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public double? TotalesVigenciaFuturaConstante { get; set; } = 0;
        public List<DeflactoresProductosConstantes> Deflactores { get; set; }
        public List<ProductosConstantesVFResumen> ResumenObjetivos { get; set; }
        public List<ProductosConstantesVFDetalle> DetalleObjetivos { get; set; }
    }

    public class ProductosConstantesVFResumen
    {
        public int? ObjetivoEspecificoid { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductosConstantesVFResumenProducto> Productos { get; set; }
    }

    public class ProductosConstantesVFResumenProducto
    {
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public double? ValorTotalVigenteFuturaProducto { get; set; } = 0;
        public double? ValorTotalVigenteFuturaProductoOriginal { get; set; } = 0;
        public double? ValorTotalVigenteFuturaProductoCorriente { get; set; } = 0;
        public double? ValorTotalVigenteFuturaProductoCorrienteOriginal { get; set; } = 0;
        public List<ProductosConstantesVFResumenVigencia> Vigencias { get; set; }
    }

    public class ProductosConstantesVFResumenVigencia
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public double? VigenciaFuturaSolicitada { get; set; }
        public double? Deflactor { get; set; }
    }

    public class ProductosConstantesVFDetalle
    {
        public int? ObjetivoEspecificoid { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string LabelBotonObjetivo { get; set; } = "+";
        public List<ProductosConstantesVFDetalleProducto> Productos { get; set; }
    }

    public class ProductosConstantesVFDetalleProducto
    {
        public int? ProductoId { get; set; }    
        public string Producto { get; set; }
        public string LabelBotonProducto { get; set; } = "+";
        public bool HabilitaEditarProducto { get; set; } = false;
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public List<ProductosConstantesVFDetalleVigencia> Vigencias { get; set; }
    }

    public class ProductosConstantesVFDetalleVigencia
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public double? Deflactor { get; set; }
        public double? ValorSolicitado { get; set; }
        public double? Decreto { get; set; }
        public double? ValorVigenteSolicitado { get; set; }
        public double? VigenteFuturasAnteriores { get; set; }
        public double? TotalVigenciasFuturaSolicitada { get; set; }
        public double? TotalVigenciasFuturaSolicitadaOriginal { get; set; }

    }

    public class DeflactoresProductosConstantes
    {
        public int? AnioConstante { get; set; }
        public double? Deflactor { get; set; }
        public int? Pagina { get; set; } = 0;
        public double? ValorTotalVigenteFutura { get; set; } = 0;
        public double? ValorTotalVigenteFuturaCorriente { get; set; } = 0;
    }
}
