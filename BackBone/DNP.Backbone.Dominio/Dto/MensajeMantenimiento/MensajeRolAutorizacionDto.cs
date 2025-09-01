using DNP.Backbone.Comunes.Dto.Base;
using System;

namespace DNP.Backbone.Dominio.Dto.MensajeMantenimiento
{
    public class MensajeRolAutorizacionDto : DtoBase<Guid>
    {
        public string NombreRol { get; set; }
        public int IdMensaje { get; set; }
    }
}
