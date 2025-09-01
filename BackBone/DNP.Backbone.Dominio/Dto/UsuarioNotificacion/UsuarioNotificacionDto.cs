using DNP.Backbone.Comunes.Dto.Base;

namespace DNP.Backbone.Dominio.Dto.UsuarioNotificacion
{
    public class UsuarioNotificacionDto : DtoBase<int>
    {
        public string IdUsuarioDNP { get; set; }
        public string NombreUsuario { get; set; }
        public int UsuarioConfigNotificacionId { get; set; }
        public UsuarioNotificacionConfigDto UsuarioConfigNotificacion { get; set; }
        public bool Visible { get; set; }
        public bool UsuarioYaLeyo { get; set; }
    }
}
