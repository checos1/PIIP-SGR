using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class RegionalizacionDto
    {
        public string BPIN { get; set; }
        public List<FuentesRegionalizacionDto> Fuentes { get; set; }
    }

    public class FuentesRegionalizacionDto
    {
        public string Etapa { get; set; }
        public string TipoFinanciadorId { get; set; }
        public string FuenteId { get; set; }
        public string TipoFinanciador { get; set; }
        public string Financiador { get; set; }
        public string Recurso { get; set; }
        public double TotalFuente { get; set; }
        public string TotalFuenteF { get; set; }
        public double TotalRegionalizadoFuente { get; set; }
        public string TotalRegionalizadoFuenteF { get; set; }
        public bool permiteHabilitar { get; set; }
        public List<ProductosRegionalizacionDto> Productos { get; set; }
    }

    public class ProductosRegionalizacionDto
    {
        public string ProductoId { get; set; }
        public string Producto { get; set; }
        public string IndicadorPrincipal { get; set; }
        public string UnidadMedidaProducto { get; set; }
        public string CantidadProducto { get; set; }
        public string CantidadProductoF { get; set; }
        public string EsAcumulativoProducto { get; set; }
        public double TotalCostoProducto { get; set; }
        public string TotalCostoProductoF { get; set; }
        public double TotalRegionalizacionProducto { get; set; }
        public string TotalRegionalizacionProductoF { get; set; }
        public bool permiteHabilitar { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<LocalizacionesRegionalizacionDto> Localizaciones { get; set; }
    }

    public class LocalizacionesRegionalizacionDto
    {
        public string LocalizacionId { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string TipoAgrupacion { get; set; }
        public string Agrupacion { get; set; }
        public bool habilitarFinal { get; set; }
        public List<VigenciasRegionalizacionDto> Vigencias { get; set; }
    }

    public class VigenciasRegionalizacionDto
    {
        public string PeriodoProyectoId { get; set; }
        public string Vigencia { get; set; }
        public double EnFirme { get; set; }
        public string EnFirmeF { get; set; }
        public double EnAjuste { get; set; }
        public string EnAjusteF { get; set; }
        public double EnAjusteOriginal { get; set; }
        public double MetaEnFirme { get; set; }
        public string MetaEnFirmeF { get; set; }
        public double MetaEnAjuste { get; set; }
        public string MetaEnAjusteF { get; set; }
        public double MetaEnAjusteOriginal { get; set; }
        public bool permiteHabilitar { get; set; }
    }
}
