namespace DNP.ServiciosNegocio.Dominio.Dto.Formulario
{
    using Productos;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ObjetivoEspecificoProductosIndicadorDto
    {
        public int? ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductoIndicadorDto> Productos { get; set; }
    }
}
