using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class EntidadesNoSGRDto
    {
        public int TipoEntidadId { get; set; }
        public int EntidadId { get; set; }
        public int TipoRecursoId { get; set; }
        public List<VigenciasValoresNoSGRDto> Vigencias { get; set; }
    }
}
