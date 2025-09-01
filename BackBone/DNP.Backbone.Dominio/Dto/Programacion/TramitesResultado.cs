using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    [ExcludeFromCodeCoverage]
    public class TramitesResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public byte[] Byte64 { get; set; }
    }
}
