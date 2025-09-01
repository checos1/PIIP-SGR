using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class UsuarioAuthDto
    {
        public string IdAplicacionDnp { get; set; }
        public Guid IdUsuario { get; set; }
        public string IdUsuarioDnp { get; set; }
        public string Nombre { get; set; }
        public string TipoIdentificacion { get; set; }
        public List<UsuarioCuentaDto> usuariocuentas { get; set; }
        public bool Activo { get; set; }
    }
}
