using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR
{
    [ExcludeFromCodeCoverage]
    public class EjecutorEntidadAsociado
    {
        public int Id { get; set; }
        public int? EjecutorId { get; set; }
        public string NitEjecutor { get; set; }
        public string NombreEntidad { get; set; }
        public string TipoEntidad { get; set; }
    }
}
