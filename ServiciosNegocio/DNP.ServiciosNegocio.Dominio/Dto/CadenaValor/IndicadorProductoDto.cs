namespace DNP.ServiciosNegocio.Dominio.Dto.CadenaValor
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class IndicadorProductoDto
    {
        public string Bpin { get; set; }
        public List<ObjetivosIndicadorProductoDto> ObjetivosEspecificos { get; set; }
    }
}
