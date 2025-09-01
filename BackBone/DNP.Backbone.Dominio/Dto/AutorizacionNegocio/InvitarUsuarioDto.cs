namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.{
    public class InvitarUsuarioDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public bool TieneModuloAdministracion { get; set; }
        public bool TieneModuloBackbone { get; set; }
        public Guid IdEntidad { get; set; }
        public Guid? IdPerfilBackbone { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }

        public string IdUsuarioDNP { get; set; }

        public bool InvitacionValida
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Nombre)
                && !string.IsNullOrWhiteSpace(Apellido)
                && !string.IsNullOrWhiteSpace(Correo)
                && !string.IsNullOrWhiteSpace(Identificacion)
                && !string.IsNullOrWhiteSpace(TipoIdentificacion);
            }
        }
    }
}
