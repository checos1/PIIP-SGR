using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    [ExcludeFromCodeCoverage]
    public class ObjetoNegocio
    {
        public string ObjetoNegocioId { get; set; }

        public string NivelId { get; set; }
        public string FlujoId { get; set; }

        public string Vigencia { get; set; }

        public string InstanciaId { get; set; }
        public string IdAccion { get; set; }
        public string IdRol { get; set; }
    }
}
