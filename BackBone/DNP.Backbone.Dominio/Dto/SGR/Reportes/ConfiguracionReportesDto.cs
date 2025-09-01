using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR.Reportes
{
    public class ConfiguracionReportesDto
    {
        public string NombreRdl { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> TipoDocumentoId { get; set; }
    }
}
