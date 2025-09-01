using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.VigenciasFuturas
{
    [ExcludeFromCodeCoverage]
    public class EliminacionAsociacionDto
    {
        public string TramiteId { get; set; }
        public string ProyectoId { get; set; }
        public string InstanciaId { get; set; }
    }
}
