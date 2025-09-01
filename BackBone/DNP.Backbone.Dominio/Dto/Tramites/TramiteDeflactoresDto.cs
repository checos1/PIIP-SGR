using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class TramiteDeflactoresDto
    {
        public int Id { get; set; }
        public int AnioBase { get; set; }
        public int AnioConstante { get; set; }
        public double Valor { get; set; }
        public string IPC { get; set; }
    }
}
