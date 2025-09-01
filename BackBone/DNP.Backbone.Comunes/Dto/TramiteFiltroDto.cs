namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TramiteFiltroDto
    {
        public string TokenAutorizacion { get; set; }
        public string IdUsuarioDNP { get; set; }
        public int TramiteId { get; set; }
        public Guid IdTipoObjetoNegocio { get; set; }
        public List<Guid> IdsRoles { get; set; }

        public List<FiltroGradeDto> FiltroGradeDtos { get; set; }

        public Guid? InstanciaId { get; set; }
        public int ProyectoId { get; set; }
        public string[] IdsEtapas { get; set; }

        public string NumeroTramite { get; set; }
        public List<int> EntidadesVisualizador { get; set; }
        public string Macroproceso { get; set; }
    }
}
