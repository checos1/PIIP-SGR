using DNP.Backbone.Comunes.Dto.Base;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.UsuarioNotificacion
{
    public class ProcedimientoAlmacenadoDto : DtoBase<int>
    {
        public string NombreProcedimiento { get; set; }
        
        public string SelectQuery { get; set; }
        
        public virtual ICollection<UsuarioNotificacionConfigDto> ConfigNotificaciones { get; set; }
    }
}
