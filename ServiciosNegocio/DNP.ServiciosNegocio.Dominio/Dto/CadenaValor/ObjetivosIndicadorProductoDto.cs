namespace DNP.ServiciosNegocio.Dominio.Dto.CadenaValor
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ObjetivosIndicadorProductoDto
    {
        public int? ObjetivoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string LabelBotonObjetivo { get; set; }
        public List<ProductosIndicadorProductoDto> Productos { get; set; }
    }
}
