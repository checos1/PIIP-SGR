using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ObjetoDatosDto
    {
        public int IdEntidad { get; set; }
        public string ObjetoJson { get; set; }
    }
}
