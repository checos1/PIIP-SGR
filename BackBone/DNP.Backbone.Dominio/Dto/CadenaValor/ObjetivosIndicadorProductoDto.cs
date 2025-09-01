using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class ObjetivosIndicadorProductoDto
    {
        public int? ObjetivoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string LabelBotonObjetivo { get; set; }
        public List<ProductosIndicadorProductoDto> Productos { get; set; }
    }
}
