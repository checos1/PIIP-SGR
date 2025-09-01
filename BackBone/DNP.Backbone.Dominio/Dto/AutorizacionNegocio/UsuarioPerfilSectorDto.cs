namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public class UsuarioPerfilSectorDto
    {
        public Guid IdUsuarioPerfilSector { get; set; }

        public Guid IdUsuarioPerfil { get; set; }

        public Guid IdRol { get; set; }

        public int IdSector { get; set; }

        public SectorNegocioDto Sector { get; set; }
    }
}
