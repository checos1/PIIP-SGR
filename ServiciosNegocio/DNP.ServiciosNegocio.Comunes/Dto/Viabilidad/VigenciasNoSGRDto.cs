using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Comunes.Dto.Viabilidad
{
    public class VigenciasNoSGRDto
    {
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public decimal Costo { get; set; }
        public List<EntidadesNoSGRDto> Entidades { get; set; }        
    }
}
