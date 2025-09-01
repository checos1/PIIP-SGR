using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR
{
    [ExcludeFromCodeCoverage]
    public class EjecutorEntidadDto
    {
        public int Id { get; set; }
        public string Nit { get; set; }
        public string TipoEntidad { get; set; }
        public string Entidad { get; set; }
    }

    public class SeccionesEjecutorEntidad
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
