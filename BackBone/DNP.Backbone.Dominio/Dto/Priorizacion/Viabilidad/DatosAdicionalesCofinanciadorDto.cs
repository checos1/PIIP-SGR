using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class DatosAdicionalesCofinanciadorDto
    {
        public int ProyectoId { get; set; }
        public int Vigencia { get; set; }
        public List<VigenciasFuentesNoSGRDto> VigenciasFuentes { get; set; }
    }
}
