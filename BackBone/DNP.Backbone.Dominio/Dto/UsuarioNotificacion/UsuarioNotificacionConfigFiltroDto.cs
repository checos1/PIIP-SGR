using DNP.Backbone.Dominio.Enums;
using System;

namespace DNP.Backbone.Dominio.Dto.UsuarioNotificacion
{
    public class UsuarioNotificacionConfigFiltroDto : UsuarioNotificacionFiltroDto
    {
        public int?[] ConfigNotificacionIds { get; set; }
        public string NombreNotificacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? EsManual { get; set; }
        public TipoConfigNotificacion? Tipo { get; set; }
        public string ContenidoNotificacion { get; set; }
        public string NombreArchivo { get; set; }
        public int? ProcedimientoAlmacenadoId { get; set; }
        public bool PantallaProgramacion { get; set; }
    }
}
