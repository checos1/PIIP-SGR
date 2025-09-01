
namespace DNP.CargaArchivos.Dominio.Dto.CargaArchivo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class TipoOperacionesDto
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }

    }
}
