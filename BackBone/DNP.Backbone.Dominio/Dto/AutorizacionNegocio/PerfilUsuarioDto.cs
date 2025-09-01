using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    public class PerfilUsuarioDto
    {
        public Guid Id { get; set; }

        public Guid IdUsuarioPerfil { get; set; }

        public string Nombre { get; set; }

        public bool Activo { get; set; }
        public Guid IdSubEntidad { get; set; }

        public string NombreSubEntidad { get; set; }
    }
}
