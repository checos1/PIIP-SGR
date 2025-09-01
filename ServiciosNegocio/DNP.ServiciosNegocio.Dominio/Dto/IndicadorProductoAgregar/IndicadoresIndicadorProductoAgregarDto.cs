using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar
{
    public class IndicadoresIndicadorProductoAgregarDto
    {
        public int? IndicadorId { get; set; }
        public string NombreIndicador { get; set; }
        public string CodigoIndicador { get; set; }
        public string IndicadorTipo { get; set; }
        public bool? IndicadorAcumula { get; set; }
        public int? IndicadorUnidadMedidaId { get; set; }
        public string NombreUnidadMedida { get; set; }
        public decimal? MetaTotal { get; set; }
    }
}
