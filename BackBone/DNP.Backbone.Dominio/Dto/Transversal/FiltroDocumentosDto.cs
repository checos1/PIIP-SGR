using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversales
{
    public class FiltroDocumentosDto
    {
        public int proyectoId { get; set; }
        public string origen { get; set; }
        public string vigencia { get; set; }
        public string periodo { get; set; }
        public string tipoDocumento { get; set; }
        public int? tramite { get; set; }
        public int? ficha { get; set; }
        public string procesoOrigen { get; set; }
        public string numeroProceso { get; set; }
        public string NombreDocumento { get; set; }
        public string proceso { get; set; }
    }
}
