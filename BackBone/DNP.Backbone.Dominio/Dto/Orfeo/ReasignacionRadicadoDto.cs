using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Orfeo
{
    [ExcludeFromCodeCoverage]
    public class ReasignacionRadicadoDto
    {
        public UsuarioRadicacionDto UsuarioOrigen { get; set; }
        public UsuarioRadicacionDto UsuarioDestino { get; set; }
        public string NoRadicado { get; set; }
        public string TramiteId { get; set; }
    }
}
