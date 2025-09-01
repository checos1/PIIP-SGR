using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio
{
    [ExcludeFromCodeCoverage]
    public class BPINsProyectosDto
    {
        public List<string> BPINs { get; set; }
        public string TokenAutorizacion { get; set; }
    }
}
