using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class ParametrosEjecucionAccionDto
    {
        public Guid IdAccion { get; set; }
        public Guid IdNivel { get; set; }
        public Guid IdFormulario { get; set; }
        public ParametrosEjecucionFlujo ParametrosEjecucionFlujo { get; set; }
    }
}
