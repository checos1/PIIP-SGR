namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    using Formulario;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Productos;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ObjetivoEspecificoDto
    {
        public int Id { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public int ObjetivoEspecificoId { get; set; }

        public int CausaId { get; set; }
        public List<ProductoCadenaValorDto> Productos { get; set; }

        public List<ProductoDto> Producto { get; set; }
    }
}
