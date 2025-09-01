using DNP.Backbone.Comunes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class ResultadoEjecucionFlujoDto
    {
        public TipoCodigoEjecucionAccion CodigoEjecucionAccion { get; set; }
        public ObjetoContextoDto ObjectoContexto { get; set; }
        public string MensajeEjecucion { get; set; }
    }
}
