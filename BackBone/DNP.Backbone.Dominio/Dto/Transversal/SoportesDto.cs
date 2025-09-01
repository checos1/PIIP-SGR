using DNP.Backbone.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    public class SoportesDto
    {
        public List<DocumentosDto> Documentos { get; set; }
        public List<int> Vigencias { get; set; }
        public List<string> Origenes { get; set; }
        public List<string> Periodos { get; set; }
        public List<string> ProcesosOrigen { get; set; }
        public List<TipoDocumento> TiposDocumento { get; set; }
    }

    public class TipoDocumento
    {
        public string NombreTipoDocumento { get; set; }
        public int TipoDocumentoId { get; set; }
    }
}
