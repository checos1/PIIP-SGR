using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    public class UsuarioPerfilDto
    {
        public Guid IdUsuarioPerfil { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdPerfil { get; set; }
        public Guid IdEntidad { get; set; }
        public string UsuarioDNP { get; set; }

        public IEnumerable<UsuarioPerfilProyectoDto> Proyectos { get; set; }
    }
}
