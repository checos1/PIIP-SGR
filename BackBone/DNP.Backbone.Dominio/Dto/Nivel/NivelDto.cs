using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Nivel
{
    public class NivelDto
    {
        public Guid IdAplicacion { get; set; }

        public string NombreTipoNivel { get; set; }

        public Guid IdNivel { get; set; }

        public string NombreNivel { get; set; }

        public Guid? IdNivelPadre { get; set; }
        public int NumeroNivel { get; set; }
    }
}
