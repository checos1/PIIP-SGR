using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    public class AlertasGeneradasFiltroDto
    {
        public ParametrosDto ParametrosDto { get; set; }
        public List<FiltroGradeDto> FiltroGradeDtos { get; set; }
    }
}
