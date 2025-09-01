namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;    
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosInboxDto
    {
        public string Aplicacion { get; set; }
        public List<Guid> ListaIdsRoles { get; set; }
        public string IdUsuario { get; set; }
        public Guid IdObjeto { get; set; }
    }
}
