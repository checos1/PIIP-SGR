using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class IndicadorProductoDto
    {
        public string Bpin { get; set; }
        public List<ObjetivosIndicadorProductoDto> ObjetivosEspecificos { get; set; }
    }
}
