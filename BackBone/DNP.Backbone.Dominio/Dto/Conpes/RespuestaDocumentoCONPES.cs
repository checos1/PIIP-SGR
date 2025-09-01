using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Conpes
{
    public class RespuestaDocumentoCONPES
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }

        public List<DocumentoCONPES> documentosCONPES { get; set; }

    }
}
