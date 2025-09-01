using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl
{
    public class GestionSeguimientoDto
    {
        public string GuidMacroproceso { get; set; }
        public string GuidInstancia { get; set; }
        public int IdProyecto { get; set; }

        public GestionSeguimientoDto()
        {
        }
    }
}
