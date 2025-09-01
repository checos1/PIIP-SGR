using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class IndicadoresIndicadorSecundarioProductoDto
    {
        public int? IndicadorId { get; set; }
        public string NombreIndicador { get; set; }
        public string CodigoIndicador { get; set; }
        public string IndicadorTipo { get; set; }
        public bool? IndicadorAcumula { get; set; }
        public int? IndicadorUnidadMedidaId { get; set; }
        public string NombreUnidadMedida { get; set; }
        public decimal? MetaTotalIndicadorMga { get; set; }
        public decimal? MetaTotalActual { get; set; }
        public decimal? MetaTotalFirme { get; set; }
        public decimal? MetaTotalFirmeAjustado { get; set; }
        public int? Marcado { get; set; }
        public string LabelBotonIndicador { get; set; }
        public bool HabilitaEditarIndicador { get; set; } = false;
        public int EsCreadoPIIP { get; set; }
        public List<VigenciaIndicadorProductoDto> Vigencias { get; set; }
    }
}
