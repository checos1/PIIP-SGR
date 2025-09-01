using DNP.Backbone.Comunes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class ResultadoDevolverFlujoDto
    {
        public bool FlujoDevuelto { get; set; }
        public TipoCodigoEjecucionAccion CodigoEjecucionAccion { get; set; }
        public string MensajeEjecucion { get; set; }

    }
}
