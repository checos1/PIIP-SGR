using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos
{
    public class AprobacionProyectoCreditoDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int EntidadId { get; set; }
        public string TipoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public Guid InstanciaId { get; set; }
        public byte IEstrategica { get; set; }
        public List<BienioDto> ComboBienios { get; set; }
        public List<TipoRecursoDto> TipoRecursos { get; set; }

        public byte? Aprobado { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaActoAdmtvo { get; set; }
    }

    public class BienioDto
    {
        public int BienioId { get; set; }
        public string Bienio { get; set; }
    }

    public class TipoRecursoDto
    {
        public int AprobacionInstanciaId { get; set; }
        public int FuenteBienioValoresId { get; set; }
        public int TipoRecursoId { get; set; }
        public string TipoRecurso { get; set; }
        public decimal ValorSolicitado { get; set; }
        public string BienioSolicitado { get; set; }
        public decimal ValorCredito { get; set; }
        public decimal CostoFinanciero { get; set; }
        public decimal PatrimonioAutonomo { get; set; }
        public List<BienioCreditoDto> Bienios { get; set; }
    }

    public class BienioCreditoDto
    {
        public int BienioId { get; set; }
        public string Bienio { get; set; }
        public decimal VrCreditoAprobado { get; set; }
        public decimal VrCostoFinanciero { get; set; }
        public decimal VrPatrimonioAutonomo { get; set; }
    }
}
