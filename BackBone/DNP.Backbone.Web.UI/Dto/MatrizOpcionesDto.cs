// ReSharper disable InconsistentNaming
namespace DNP.Backbone.Web.UI.Dto
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MatrizOpcionesDto
    {
        public string IdOpcionDnp { get; set; }
        public string link { get; set; }

        public string img { get; set;  }

        public string icono { get; set;  }
    }
}
