using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class IndicadoresIndicadorProductoDto
    {
        public int? IndicadorId { get; set; }
        public string NombreIndicador { get; set; }
        public string CodigoIndicador { get; set; }
        public string IndicadorTipo { get; set; }
        public bool? IndicadorAcumula { get; set; }
        public int? IndicadorUnidadMedidaId { get; set; }
        public string NombreUnidadMedida { get; set; }
        public int? Firme { get; set; }
        public double? MetaTotalIndicadorMga { get; set; }
        public double? MetaTotalActual { get; set; }
        public double? MetaTotalFirme { get; set; }
        public double? MetaTotalFirmeAjustado { get; set; }
        public string IndicadorAcumulaAjustado { get; set; }
        public string LabelBotonIndicador { get; set; }
        public bool HabilitaEditarIndicador { get; set; } = false;
        public int EsCreadoPIIP { get; set; }
        public bool? IndicadorAcumulaOriginal { get; set; }
        public string IndicadorAcumulaAjustadoOriginal { get; set; }
        public decimal? MetaTotalFirmeAjustadoOriginal { get; set; }
        public List<VigenciaIndicadorProductoDto> Vigencias { get; set; }
    }
}
