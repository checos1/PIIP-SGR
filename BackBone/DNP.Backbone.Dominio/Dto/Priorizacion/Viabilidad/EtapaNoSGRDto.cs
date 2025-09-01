using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class EtapaNoSGRDto
    {
        public int EtapaId { get; set; }
        public int ProyectoId { get; set; }
        public List<VigenciasNoSGRDto> Vigencias { get; set; }
    }
}
