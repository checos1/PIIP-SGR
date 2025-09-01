namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class AjustesPoliticaTMetasDto
    {
        public string BPIN { get; set; }
        public int? ProyectoId { get; set; }
        public List<PoliticaAjustesPoliticaTMetasDto> POLITICAS { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticaAjustesPoliticaTMetasDto
    {
        public int? PoliticaId { get; set; }
        public string Politica { get; set; }
        public List<ProductosAjustesPoliticaTMetasDto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProductosAjustesPoliticaTMetasDto
    {
        public int? ProductoId { get; set; }
        public string Producto { get; set; }
        public int? IndicadorId { get; set; }
        public string Indicador { get; set; }
        public int? UnidaddeMedidaId { get; set; }
        public string UnidaddeMedida { get; set; }
        public decimal? MetaTotalProducto { get; set; }
        public List<VigenciaAjustesPoliticaTMetasDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaAjustesPoliticaTMetasDto
    {
        public int? VIGENCIA { get; set; }
        public int? ProgramacionIndicadorId { get; set; }
        public decimal? MetaTotalProductoVigencia { get; set; }
        public decimal? MetaTotalProductoPolitica { get; set; }
        public List<LocalizacionesAjustesPoliticaTMetasDto> Localizaciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LocalizacionesAjustesPoliticaTMetasDto
    {
        public int? LocalizacionId { get; set; }
        public string Localizacion { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public decimal? MetaProductoLocalizacionVigenteFirme { get; set; }
        public decimal? MetaProductoLocalizacionVigenteAjuste { get; set; }
        public decimal? MetaProductoLocalizacionSolicitadoFirme { get; set; }
        public decimal? MetaProductoLocalizacionSolicitadoAjuste { get; set; }
        public decimal? MetaProductoPoliticaVigenteFirme { get; set; }                
        public decimal? MetaProductoPoliticaSolicitadoFirme { get; set; }
        public decimal? MetaProductoPoliticaAjuste { get; set; }
    }
}
