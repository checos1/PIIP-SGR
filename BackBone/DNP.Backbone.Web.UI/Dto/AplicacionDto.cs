namespace DNP.Backbone.Web.UI.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AplicacionDto
    {
        public Guid IdAplicacion { get; set; }
        public string IdAplicacionDnp { get; set; }
        public string Nombre { get; set; }

        public  string Funcionalidad { get; set; }

    }
}
