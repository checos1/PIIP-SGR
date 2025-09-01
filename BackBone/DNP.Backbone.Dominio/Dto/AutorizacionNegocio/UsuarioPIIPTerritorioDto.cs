namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Collections.Generic;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.{
    public class UsuarioPIIPTerritorioDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public bool TieneModuloAdministracion { get; set; }
        public bool TieneModuloBackbone { get; set; }
        public Guid IdEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public List<UsuarioPerfilDto> UsuarioPerfil { get; set; }
        public string NombrePerfil{ get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string IdUsuarioDNP { get; set; }
        public int TipoInvitacion { get; set; }

        public bool RegistroValido
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
