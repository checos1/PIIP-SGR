using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Indicadores
{
    public class IndicadorRegionalizacionDto
    {
        public int? IndicadorId { get; set; }
        public int? RegionalizacionMetasId { get; set; }
        public int? RegionId { get; set; }
        public string RegionNombre { get; set; }
        public string RegionCodigo { get; set; }
        public int? DepartamentoId { get; set; }
        public string DepartamentoNombre { get; set; }
        public string DepartamentoCodigo { get; set; }
        public int? MunicipioId { get; set; }
        public string MunicipioNombre { get; set; }
        public string MunicipioCodigo { get; set; }
        public int? AgrupacionId { get; set; }
        public string AgrupacionNombre { get; set; }
        public string AgrupacionCodigo { get; set; }
        public decimal? MetaVigente { get; set; }
        public decimal? MetaRezago { get; set; }
        public decimal? AvanceVigencia { get; set; }
        public decimal? AvanceRezago { get; set; }
    }
}
