using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class CerrarRadicadoDto
    {
        public UsuarioRadicacionDto UsuarioArchiva { get; set; }
        public decimal NoRadicado { get; set; }
        public string Observacion { get; set; }
    }
}
