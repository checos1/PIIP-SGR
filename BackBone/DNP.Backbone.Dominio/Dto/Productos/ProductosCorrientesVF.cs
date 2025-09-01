using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Productos
{
    public class ProductosCorrientesVF
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public double? TotalesVigenciaFuturaCorriente { get; set; }
        public List<AniosVigenciasProductosCorrientes> AniosVigencias { get; set; }
        public List<ProductosCorrientesVFResumen> ResumenObjetivos { get; set; }
        public List<ProductosCorrientesVFDetalle> DetalleObjetivos { get; set; }
    }

    public class ProductosCorrientesVFResumen
    {
        public int? ObjetivoEspecificoid { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductosCorrientesVFResumenProducto> Productos { get; set; }
    }

    public class ProductosCorrientesVFResumenProducto
    {
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public double? ValorTotalVigenteFuturaProducto { get; set; } = 0;
        public double? ValorTotalVigenteFuturaProductoOriginal { get; set; } = 0;
        public List<ProductosCorrientesVFResumenVigencia> Vigencias { get; set; }
    }

    public class ProductosCorrientesVFResumenVigencia
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public double? VigenciaFuturaSolicitada { get; set; }
    }

    public class ProductosCorrientesVFDetalle
    {
        public int? ObjetivoEspecificoid { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string LabelBotonObjetivo { get; set; } = "+";
        public List<ProductosCorrientesVFDetalleProducto> Productos { get; set; }
    }

    public class ProductosCorrientesVFDetalleProducto
    {
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public string LabelBotonProducto { get; set; } = "+";
        public bool HabilitaEditarProducto { get; set; } = false;
        public int? TramiteId { get; set; }
        public int? ProyectoId { get; set; }
        public List<ProductosCorrientesVFDetalleVigencia> Vigencias { get; set; }
    }

    public class ProductosCorrientesVFDetalleVigencia
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public double? ValorSolicitado { get; set; }
        public double? Decreto { get; set; }
        public double? ValorVigenteSolicitado { get; set; }
        public double? VigenteFuturasAnteriores { get; set; }
        public double? TotalVigenciasFuturaSolicitada { get; set; }
        public double? TotalVigenciasFuturaSolicitadaOriginal { get; set; }

    }

    public class AniosVigenciasProductosCorrientes
    {
        public int? AnioConstante { get; set; }
        public int? Pagina { get; set; } = 0;
        public double? ValorTotalVigenteFutura { get; set; } = 0;
    }
}

