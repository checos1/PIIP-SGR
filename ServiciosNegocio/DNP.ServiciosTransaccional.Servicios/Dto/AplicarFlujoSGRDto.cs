using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class AplicarFlujoSGRDto
    {
        public string ObjetoNegocioId { get; set; }
        public Guid instanciaId { get; set; }
        public string Usuario { get; set; }
    }
}
