using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class VigenciasNoSGRDto
    {
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public decimal Costo { get; set; }
        public List<EntidadesNoSGRDto> Entidades { get; set; }
    }
}
