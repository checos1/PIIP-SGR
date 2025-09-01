using System;
using System.Collections.Generic;

namespace DNP.Backbone.Comunes.Dto
{
    public class InstanciaTramiteDto
    {
        public ParametrosInboxDto ParametrosInboxDto { get; set; }
        public TramiteFiltroDto TramiteFiltroDto { get; set; }
        public string[] ColumnasVisibles { get; set; }
        public Guid? InstanciaId { get; set; }
        public List<int> EntidadesVisualizador { get; set; }
    }
}
