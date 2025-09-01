using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class ParametrosEjecucionFlujo
    {
        public Guid IdInstanciaFlujo { get; set; }
        public Guid IdAccion { get; set; }
        public bool PostDefinitivo { get; set; }
        public string DireccionIp { get; set; }
        public int? ResourceGroupId { get; set; }
        public ObjetoContextoDto ObjetoContexto { get; set; }
        public ObjetoDatosDto ObjetoDatos { get; set; }
    }
}
