namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public class UsuarioPerfilProyectoDto
    {
        public Guid IdUsuarioPerfilProyecto { get; set; }

        public Guid IdUsuarioPerfil { get; set; }

        public int IdProyecto { get; set; }
    }
}
