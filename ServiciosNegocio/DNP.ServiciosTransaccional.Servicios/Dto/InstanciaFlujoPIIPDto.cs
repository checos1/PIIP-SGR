using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class InstanciaFlujoPIIPDto
    {
        public string FlujoId { get; set; }
        public string ObjetoId { get; set; }
        public string UsuarioId { get; set; }
        public string RolId { get; set; }
        public string TipoObjetoId { get; set; }
        public List<string> ListaEntidades { get; set; }
    }
}
