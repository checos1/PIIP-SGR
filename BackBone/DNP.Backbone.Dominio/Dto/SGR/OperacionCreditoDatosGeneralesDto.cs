using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGR
{
    public class OperacionCreditoDatosGeneralesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int FuentesCredito { get; set; }
        public List<CriteriosDto> Criterios { get; set; }
    }

    public class CriteriosDto
    {
        public string NombreTipoValor { get; set; }
        public bool? Habilita { get; set; }
        public decimal? Valor { get; set; }
    }
}
