using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR
{
    [ExcludeFromCodeCoverage]
    public class EjecutorCatalogoDto : CatalogoDto
    {
        public int? IdEjecutor { get; set; }
        public string Nit { get; set; }
        public string Entidad { get; set; }
    }
}
