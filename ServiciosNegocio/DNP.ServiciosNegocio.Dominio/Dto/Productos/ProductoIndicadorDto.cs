namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Indicadores;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ProductoIndicadorDto
    {     
        public int? ProductoId { get; set; }  
        public string Producto { get; set; }       
        public List<IndicadorDto> Indicadores { get; set; }
    }
}
