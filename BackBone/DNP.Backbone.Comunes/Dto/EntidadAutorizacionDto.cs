namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EntidadAutorizacionDto
    {
        public Guid IdEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public int? IdEntidadMGA { get; set; }
        public string TipoEntidad { get; set; }
        public RolAutorizacionDto Roles { get; set; }
    }
}
