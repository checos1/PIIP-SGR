using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Transversales
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ErroresTramiteDto
    {
        public string Seccion { get; set; }
        public string Capitulo { get; set; }
        public string Errores { get; set; }

    }
}
