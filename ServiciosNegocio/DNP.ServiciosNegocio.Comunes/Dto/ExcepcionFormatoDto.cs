using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class ExcepcionFormatoDto
    {
        public string Excepcion { get; set; }

        public List<string> Detalle { get; set; }
    }
}
