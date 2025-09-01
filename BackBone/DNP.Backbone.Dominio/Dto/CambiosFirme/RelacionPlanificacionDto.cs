using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto
{
    public class RelacionPlanificacionDto
    {
        public int Id { get; set; }
        public string NumeroConpes { get; set; }
        public string NombreConpes { get; set; }
        public string Estado { get; set; }

    }

    public class RelacionPlanificacionCambiosDto
    {
        public List<RelacionPlanificacionDto> Nuevos { get; set; }
        public List<RelacionPlanificacionDto> Eliminados { get; set; }

    }
}
