using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class ProyectoFiltroVerificacionOcadPazSgrDto : ProyectoFiltroDto
    {
        public string[] IdsSubPasosVerificacion { get; set; }
        public int? ValidarActual { get; set; }
    }
}
