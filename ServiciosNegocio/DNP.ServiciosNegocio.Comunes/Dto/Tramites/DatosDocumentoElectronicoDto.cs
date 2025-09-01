using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class DatosDocumentoElectronicoDto
    {
        public string fileBase64Bin { get; set; }
        public string extension { get; set; }
        public string nombre { get; set; }
    }
}
