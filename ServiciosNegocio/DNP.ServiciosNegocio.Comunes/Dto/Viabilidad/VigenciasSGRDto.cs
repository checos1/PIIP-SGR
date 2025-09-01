using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Comunes.Dto.Viabilidad
{
    public class VigenciasSGRDto
    {
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public decimal Costo { get; set; }
        public List<EntidadesSGRDto> Entidades { get; set; }
    }
}
