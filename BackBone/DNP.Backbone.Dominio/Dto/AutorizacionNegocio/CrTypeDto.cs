using System.Diagnostics.CodeAnalysis;

namespace DNP.Autorizacion.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class CrTypeDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
