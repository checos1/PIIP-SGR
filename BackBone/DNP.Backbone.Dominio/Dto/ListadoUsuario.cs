using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    public class ListadoUsuario
    {
        public string IdUsuarioDNP { get; set; }
        public string NombreUsuario { get; set; }
        public string IdRol { get; set; }
        public string NombreRol { get; set; }
        public string IdEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public int IdEntidadMGA { get; set; }
    }
}
