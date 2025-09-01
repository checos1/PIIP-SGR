using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class OperacionCreditoDetallesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public decimal? ValorTotalCredito { get; set; }
        public decimal? CostoFinanciero { get; set; }
        public decimal? CostoPatrimonio { get; set; }
        public List<ValoresCreditoDto> ValoresCredito { get; set; }
        public List<FuentesAdicionalesDto> FuentesAdicionales { get; set; }
    }

    public class ValoresCreditoDto
    {
        public int FuenteId { get; set; }
        public string Etapa { get; set; }
        public string TipoEntidad { get; set; }
        public string Entidad { get; set; }
        public string TipoRecurso { get; set; }
        public short Habilita { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ValorCredito { get; set; }
        public decimal? CostoFinanciero { get; set; }
        public decimal? CostoPatrimonio { get; set; }
        public decimal? ValorServicioDeuda { get; set; }
        public decimal? ValorTotalCredito { get; set; }
    }

    public class FuentesAdicionalesDto
    {
        public int FuenteAdicionalCreditoId { get; set; }
        public int IdEtapa { get; set; }
        public string Etapa { get; set; }
        public int IdTipoEntidad { get; set; }
        public string TipoEntidad { get; set; }
        public int IdEntidad { get; set; }
        public string Entidad { get; set; }
        public int IdTipoRecurso { get; set; }
        public string TipoRecurso { get; set; }
        public decimal? CostoFinanciero { get; set; }
        public decimal? CostoPatrimonio { get; set; }
    }
}
