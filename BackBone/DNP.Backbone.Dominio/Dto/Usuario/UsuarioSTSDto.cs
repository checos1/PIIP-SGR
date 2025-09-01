using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    public class UsuarioSTSDto
    {
        public string pTipoDocumento { get; set; }
        public string pNumeroDocumento { get; set; }
        public string pPassword { get; set; }
        public string pCorreo { get; set; }
        public string pAplicacion { get; set; }
    }
}
