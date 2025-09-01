
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProductoIndicadorDetalleDto
    {
        public int? IndicadorId { get; set; }
        public int? IndicadorBisId { get; set; }
        public int? IndicadorUnidadMedidaId { get; set; }
        public int? IndicadorProgramaId { get; set; }
        public int? IndicadorTipoOrigenId { get; set; }
        public string IndicadorProducto { get; set; }
        public decimal? IndicadorMetaTotal { get; set; }
        public string IndicadorFuenteVerificacionId { get; set; }
        public bool? IndicadorAcumula { get; set; }
        public bool? IndicadorAsociado { get; set; }
        public bool? IndicadorDelPrograma { get; set; }
        public string IndicadorTipo { get; set; }
        public bool? IndicadorProyectoBase { get; set; }
        public int? ProductoId { get; set; }

        public List<MetaDto> Metas { get; set; }
    }
}
