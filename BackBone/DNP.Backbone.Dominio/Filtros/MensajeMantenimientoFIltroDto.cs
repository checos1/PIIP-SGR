using DNP.Backbone.Dominio.Enums;
using System;

namespace DNP.Backbone.Dominio.Filtros.MensajeMantenimiento
{
    public class MensajeMantenimientoFiltroDto
    {
        public int?[] Ids { get; set; }
        public string[] TiposEntidades { get; set; }
        public Guid?[] EntidadesIds { get; set; }
        public string NombreMensaje { get; set; }
        public DateTime? FechaCreacionInicio { get; set; }
        public DateTime? FechaCreacionFin { get; set; }
        public EstadoMensajeMantenimiento?[] EstadosMensajes { get; set; }
        public string MensajeTemplate { get; set; }
        public TipoMensajeMantenimiento?[] TiposMensajes { get; set; }
        public bool? TieneRestringeAcesso { get; set; }
        public Guid?[] RolesIds { get; set; }
        public bool ComprobarMensajes { get; set; } = false;
        public DateTime? FechaComprobacion { get; set; }
        public bool PantallaProgramacion { get; set; }
    }
}
