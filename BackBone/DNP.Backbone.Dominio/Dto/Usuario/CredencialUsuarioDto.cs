using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Usuario
{
    public class CredencialUsuarioDto
    {
        public string Usuario { get; set; }
        public string ClaveActual { get; set; }
        public string NuevaClave { get; set; }
        public string Oid { get; set; }
        public string TenantId { get; set; }
    }
}
