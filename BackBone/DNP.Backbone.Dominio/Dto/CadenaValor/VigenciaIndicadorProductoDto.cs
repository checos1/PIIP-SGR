using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class VigenciaIndicadorProductoDto
    {
        public int? Vigencia { get; set; }
        public double? MetaVigenciaIndicadorMga { get; set; }
        public double? MetaVigenciaIndicadorFirme { get; set; }
        public double? MetaVigencialIndicadorAjuste { get; set; }
        public double? MetaVigencialIndicadorAjusteOriginal { get; set; }
    }
}
