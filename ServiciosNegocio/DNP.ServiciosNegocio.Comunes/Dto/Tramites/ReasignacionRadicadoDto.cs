using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
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
