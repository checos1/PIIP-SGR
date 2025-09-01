namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DNP.Autorizacion.Dominio.Dto;

    [ExcludeFromCodeCoverage]
    public class UsuarioDto : DtoBase
    {
        public Guid IdAplicacion { get; set; }
        public string IdAplicacionDnp { get; set; }

        public Guid IdUsuario { get; set; }

        public string IdUsuarioDnp { get; set; }

        public string Nombre { get; set; }

        public string TipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public bool Activo { get; set; }

        public bool Administrador { get; set; }

        public List<PerfilDto> Perfiles { get; set; }

        public ICollection<UsuarioPerfilDto> UsuarioPerfil { get; set; }

        public List<UsuarioCuentaDto> UsuarioCuentas { get; set; }

        public bool Seleccionado { get; set; }
        public string Correo { get; set; }
        public Guid IdEntidad { get; set; }
        public string UsuarioDnp { get; set; }
    }
    public abstract class DtoBase
    {
        public DateTime? FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
    }
}
