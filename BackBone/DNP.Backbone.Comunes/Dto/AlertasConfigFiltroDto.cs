using System.Collections.Generic;

namespace DNP.Backbone.Comunes.Dto
{
    public class AlertasConfigFiltroDto
    {
        public ParametrosDto ParametrosDto { get; set; }
        public AlertasConfigDto AlertasConfigDto { get; set; }
        public List<FiltroGradeDto> FiltroGradeDtos { get; set; }
    }
}
