namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    using System;
    using System.Collections.Generic;

    public class DesagregarRegionalizacionDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public  bool TieneDeMasUnaFuente { get; set; }
        public string GuiMacroproceso { get; set; }
        public Guid InstanciaId { get; set; }
        public List<ProductosRegionalizacion> Productos { get; set; }
    }

    public class ProductosRegionalizacion
    {
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public int? ProductCatalogId { get; set; }
        public List<FuentesRegionalizacion> Fuentes { get; set; }
    }

    public class FuentesRegionalizacion
    {
        public int? FuenteId { get; set; }
        public string Etapa { get; set; }
        public int? TipoFinanciadorId { get; set; }
        public string TipoFinanciador { get; set; }
        public int? SectorId { get; set; }
        public string Sector { get; set; }
        public string Financiador { get; set; }
        public string Recurso { get; set; }
        public decimal? TotalFuente { get; set; }
        public List<LocalizacionRegionalizacion> Localizacion { get; set; }
    }

    public class LocalizacionRegionalizacion
    {
        public int? LocalizacionId { get; set; }
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public int? AgrupacionId { get; set; }
        public string Agrupacion { get; set; }
        public List<VigenciaRegionalizacion> Vigencias { get; set; }
    }
    public class VigenciaRegionalizacion
    {
        public int? Vigencia { get; set; }
        public decimal? CostoMGA { get; set; }
        public decimal? RegionalizadoMGA { get; set; }
        public decimal? MetaMGA { get; set; }
        public decimal? ValorFuente { get; set; }
        public double? Totalsolicitado { get; set; }
    }
}
