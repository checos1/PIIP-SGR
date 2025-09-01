namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class UsuarioTramite
    {
        public Guid IdUsuario { get; set; }
        public string IDUsuarioDNP { get; set; }
        public string NombreUsuario { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public bool ActivoUsuario { get; set; }
    }

}
