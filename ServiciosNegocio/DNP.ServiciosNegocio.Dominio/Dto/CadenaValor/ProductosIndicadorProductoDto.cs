namespace DNP.ServiciosNegocio.Dominio.Dto.CadenaValor
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
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
