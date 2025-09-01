using DNP.Backbone.Comunes.Dto.Base;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.UsuarioNotificacion
{
    public class UsuarioNotificacionConfigDto : DtoBase<int>
    {
        public string NombreNotificacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsManual { get; set; }
        public string ContenidoNotificacion { get; set; }
        public TipoConfigNotificacion Tipo { get; set; }
        public string NombreTipo { get { return Tipo.GetDisplayName(); }  }
        
        public string NombreArchivo { get; set; }
        public int? ProcedimientoAlmacenadoId { get; set; }
        public ProcedimientoAlmacenadoDto ProcedimientoAlmacenado { get; set; }
        public ICollection<UsuarioNotificacionDto> UsuarioNotificaciones { get; set; }
        public string IdArchivo { get; set; }
    }
}
