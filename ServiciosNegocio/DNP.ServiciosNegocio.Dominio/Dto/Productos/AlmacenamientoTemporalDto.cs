using System;

namespace DNP.ServiciosNegocio.Dominio.Dto.Productos
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AlmacenamientoTemporalDto
    {
        public Guid AccionId { get; set; }
        public Guid InstanciaId { get; set; }
        public string Json { get; set; }
    }
}
