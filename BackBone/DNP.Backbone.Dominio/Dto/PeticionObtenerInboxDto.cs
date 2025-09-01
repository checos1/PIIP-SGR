using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PeticionObtenerInboxDto
    {
        public string IdUsuario { get; set; }
        public string IdObjeto { get; set; }
        public string Aplicacion { get; set; }
    }
}