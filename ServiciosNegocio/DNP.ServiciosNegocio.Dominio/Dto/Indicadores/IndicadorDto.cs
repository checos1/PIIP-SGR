using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Indicadores
{
    public class IndicadorDto
    {
        public int? IndicadorId { get; set; }
        public int? ProductoId { get; set; }
        public string IndicadorNombre { get; set; }
        public string IndicadorCodigo { get; set; }
        public string IndicadorTipo { get; set; }
        public bool? IndicadorAcumula { get; set; }
        public int? IndicadorUnidadMedidaId { get; set; }
        public string IndicadorNombreUnidadMedida { get; set; }
        public decimal? IndicadorMetaTotal { get; set; }
        public decimal? IndicadorMetaVigente { get; set; }
        public decimal? IndicadorMetaRezago { get; set; }
        public decimal? IndicadorAvanceVigencia { get; set; }
        public decimal? IndicadorAvanceRezago { get; set; }
        public decimal? IndicadorAvanceAcumulado { get; set; }
        public string IndicadorObservacion { get; set; }
        public List<IndicadorRegionalizacionDto> Regionalizacion { get; set; }
    }
}
