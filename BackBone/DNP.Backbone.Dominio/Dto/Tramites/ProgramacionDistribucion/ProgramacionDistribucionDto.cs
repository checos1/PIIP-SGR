using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion
{
    public class ProgramacionDistribucionDto
    {
        public int? TramiteId { get; set; }
        public string NivelId { get; set; }
        public int? EntidadDestinoId { get; set; }
        public int? SeccionCapitulo { get; set; }
        public List<ValoresProgramacion> ValoresProgramacion { get; set; }
    }

    public class ValoresProgramacion
    {
        public int? ProyectoId { get; set; }
        public decimal? DistribucionCuotaComunicadaNacionCSF { get; set; }
        public decimal? DistribucionCuotaComunicadaNacionSSF { get; set; }
        public decimal? DistribucionCuotaComunicadaPropios { get; set; }
    }
}
