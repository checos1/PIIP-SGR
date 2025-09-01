using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.UsuarioNotificacion
{
    public class UsuarioNotificacionMensajesPDFDto
    {
        public DateTime? Fecha { get; set; }
        public string Notificacion { get; set; }
        public string NombreNotificacion { get; set; }
        public string Estado { get; set; }
    }
}
