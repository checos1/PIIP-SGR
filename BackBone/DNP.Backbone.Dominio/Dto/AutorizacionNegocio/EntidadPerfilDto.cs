using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    public class EntidadPerfilDto : EntidadFiltroDto
    {
        public EntidadPerfilDto()
        {
            Perfiles = new List<PerfilUsuarioDto>();
        }

        public IList<PerfilUsuarioDto> Perfiles { get; set; }
    }
}
