using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class ParametrosDevolverFlujoDto
    {
        public Guid IdInstanciaFlujo { get; set; }

        public Guid IdAccionOrigen { get; set; }

        public Guid IdAccionDestino { get; set; }

        public Guid IdAplicacion { get; set; }
    }
}
