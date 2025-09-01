// ReSharper disable InconsistentNaming
namespace DNP.Backbone.Web.UI.Dto
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class NombreInicioSesionDto
    {

        public string type { get; set; }
        public string value { get; set; }

    }
}