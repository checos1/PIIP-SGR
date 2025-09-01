using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EntidadDto
    {
        public int EntidadId { get; set; }
        public string EntidadNombre { get; set; }
        public string TipoEntidad { get; set; }
    }
}
