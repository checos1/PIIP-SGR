using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.CadenaValor
{
    public class IndicadorCapituloModificadoDto
    {
        public string CodigoIndicador { get; set; }
        public bool? EsPrincipal { get; set; }
        public string TipoIndicador { get; set; }
        public bool? EsAcumulable { get; set; }
        public string IndicadorAcumula { get; set; }
        public string NombreIndicador { get; set; }
        public string Ajuste { get; set; }
        public int? Vigencia { get; set; }
        public string MetaEnFirme { get; set; }
        public string MetaEnAjuste { get; set; }
        public string Diferencia { get; set; }
        public string Mensaje { get; set; }
        public string ClaseCSS { get; set; }
    }
}
