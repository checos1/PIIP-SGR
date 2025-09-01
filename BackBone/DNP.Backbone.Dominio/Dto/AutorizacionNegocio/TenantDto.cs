using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    public class TenantDto
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string Nombre { get; set; }
    }
}
