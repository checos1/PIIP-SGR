using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class DatosRadicadoDto
    {
        public bool esPrincipal { get; set; }
        public decimal NoRadicado { get; set; }
        public string observacion { get; set; }
        public string codigoAnexo { get; set; }
    }
}
