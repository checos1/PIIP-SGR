using DNP.Backbone.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CentroAyuda
{
    public class AyudaTemaFiltroDto
    {
        public AyudaTipoEnum Tipo { get; set; }

        public bool SoloActivos { get; set; }
    }
}
