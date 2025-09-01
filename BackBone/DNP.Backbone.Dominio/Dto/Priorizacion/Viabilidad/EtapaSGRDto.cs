using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class EtapaSGRDto
    {
        public int EtapaId { get; set; }
        public int ProyectoId { get; set; }
        public List<VigenciasSGRDto> Vigencias { get; set; }
    }
}
