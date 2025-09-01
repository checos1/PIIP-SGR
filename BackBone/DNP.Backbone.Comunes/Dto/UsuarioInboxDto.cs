using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    using System;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UsuarioInboxDto
    {
        public Guid IdUsuario { get; set; }
        public string IdUsuarioDnp { get; set; }
    }
}
