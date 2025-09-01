namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ParametrosObjetosNegocioInboxDto
    {
        public Guid IdTipoObjetoNegocio { get; set; }
        public string IdUsuarioDNP { get; set; }
        public List<Guid> IdsRoles { get; set; }
        public string TokenAutorizacion { get; set; }

        public ProyectoFiltroDto ProyectoFiltro { get; set; }
    }
}
