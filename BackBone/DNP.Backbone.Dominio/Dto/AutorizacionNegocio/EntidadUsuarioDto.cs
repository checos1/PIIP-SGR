using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    public class EntidadUsuarioDto : EntidadFiltroDto
    {
        public IEnumerable<U> Usuarios { get; set; }

        public class U
        {
            public Guid Id { get; set; }

            public string IdUsuarioDNP { get; set; }

            public Guid IdUsuarioPerfil { get; set; }

            public string Nombre { get; set; }

            public bool Activo { get; set; }


            public string Correo { get; set; }
            public string TipoIdentificacion { get; set; }
            public string Identificacion { get; set; }
        }
    }
}
