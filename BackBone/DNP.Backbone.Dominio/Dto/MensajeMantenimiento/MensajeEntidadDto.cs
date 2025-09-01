using DNP.Backbone.Comunes.Dto.Base;
using System;

namespace DNP.Backbone.Dominio.Dto.MensajeMantenimiento
{
    public class MensajeEntidadDto : DtoBase<Guid>
    {
        public string NombreEntidad { get; set; }
        public int IdMensaje { get; set; }
    }
}
