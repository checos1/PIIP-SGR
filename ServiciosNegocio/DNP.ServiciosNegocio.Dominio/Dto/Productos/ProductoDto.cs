namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProductoDto
    {
        public int? ProductoTipoMedidaId { get; set; }
        public int? ProductoId { get; set; }
        public int? ProductoCatalogoId { get; set; }
        public int? ProductoAlternativaId { get; set; }
        public int ObjetivoEspecificoId { get; set; }

        public string Producto { get; set; }
        public string ProductoComplemento { get; set; }
        public decimal? ProductoCantidad { get; set; }
        public List<ProductoIndicadorDetalleDto> ProductoIndicadorDetalle { get; set; }

    }
}
