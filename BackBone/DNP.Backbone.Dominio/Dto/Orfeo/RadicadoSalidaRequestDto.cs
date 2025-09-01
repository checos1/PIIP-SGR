using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Orfeo
{
    [ExcludeFromCodeCoverage]
    public class RadicadoSalidaRequestDto
    {
        public string NumeroTramite { get; set; }
        public string RadicadoSalida { get; set; }
        public string NumeroExpediente { get; set; }
    }
}
