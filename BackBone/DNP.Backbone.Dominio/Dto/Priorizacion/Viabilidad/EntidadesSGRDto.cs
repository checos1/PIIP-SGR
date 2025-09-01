using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class EntidadesSGRDto
    {
        public int TipoEntidadId { get; set; }
        public int EntidadId { get; set; }
        public int TipoRecursoId { get; set; }
        public List<BieniosSGRDto> Bienios { get; set; }
    }
}
