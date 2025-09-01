namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class UsuarioCuentaDto : DtoBase
    {
        public int Id { get; set; }
        public Guid IdUsuario { get; set; }
        public UsuarioDto Usuario { get; set; }
        public string Cuenta { get; set; }
        public Guid TenantId { get; set; }
        public TenantDto Tenant { get; set; }
        public Guid? EntidadId { get; set; }

    }
}
