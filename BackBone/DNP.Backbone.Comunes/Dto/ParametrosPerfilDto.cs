using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class ParametrosPerfilDto
    {
        public Guid IdPerfil { get; set; }
        public Guid IdUsuarioPerfil { get; set; }
        public Guid IdAplicacion { get; set; }
        public List<Guid> IdsUsuarioPerfil { get; set; }
    }
}
