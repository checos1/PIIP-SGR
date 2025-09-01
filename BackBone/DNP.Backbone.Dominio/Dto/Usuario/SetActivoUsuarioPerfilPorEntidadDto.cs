using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    public class SetActivoUsuarioPerfilPorEntidadDto
    {
        public Guid IdUsuario { get; set; }

        public Guid IdEntidad { get; set; }

        public bool Activo { get; set; }

        public string UsuarioDnp { get; set; }
        public string TipoEntidad { get; set; }
        public string IdUsuarioDnp { get; set; }
    }
}
