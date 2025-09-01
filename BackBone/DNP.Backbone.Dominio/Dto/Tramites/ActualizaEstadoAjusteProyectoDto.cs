using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class ActualizaEstadoAjusteProyecto
    {
        public string ObjetoNegocioId { get; set; }
        public int TramiteId { get; set; }
        public string Observacion { get; set; }
        public string tipoDevolucion { get; set; }
      
    }
}
