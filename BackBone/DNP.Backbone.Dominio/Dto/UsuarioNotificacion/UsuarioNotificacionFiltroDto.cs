namespace DNP.Backbone.Dominio.Dto.UsuarioNotificacion
{
    public class UsuarioNotificacionFiltroDto
    {
        public int?[] UsuarioNotificacionIds { get; set; }
        public string IdUsuarioDNP { get; set; }
        public bool? Visible { get; set; }
        public bool? NotificacionesLeida { get; set; }
    }
}
