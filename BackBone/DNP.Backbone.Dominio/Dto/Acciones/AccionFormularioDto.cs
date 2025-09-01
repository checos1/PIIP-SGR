using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Acciones
{
    public class AccionFlujoDto
    {
        public Guid IdInstancia { get; set; }
        public Guid IdAcccion { get; set; }
        public string ObjetoNegocioId { get; set; }
        public string UsuarioDNP { get; set; }
        public string ObjetoJson { get; set; }
    }
}
