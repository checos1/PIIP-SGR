using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class UsuarioDto
    {
        public string Nombre { get; set; }

        public Guid IdUsuario { get; set; }

        public string IdUsuarioDnp { get; set; }

        public List<UsuarioCuenta> UsuarioCuentas { get; set; }

    }

    public class UsuarioCuenta
    {
        public string Cuenta { get; set; }
    }
}
