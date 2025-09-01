using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    public class SetActivoUsuarioPerfilDto
    {
        public Guid IdUsuarioPerfil { get; set; }

        public bool Activo { get; set; }

        public string UsuarioDnp { get; set; }
        public Guid IdEntidad { get; set; }
    }
}
