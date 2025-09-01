using DNP.Backbone.Comunes.Dto.Base;
using DNP.Backbone.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.MensajeMantenimiento
{
    public class MensajeMantenimientoDto : DtoBase<int>
    {
        public string TipoEntidad { get; set; }
        public string NombreMensaje { get; set; }
        public DateTime FechaCreacionInicio { get; set; }
        public DateTime FechaCreacionFin { get; set; }
        public EstadoMensajeMantenimiento EstadoMensaje { get; set; }
        public string MensajeTemplate { get; set; }
        public TipoMensajeMantenimiento TipoMensaje { get; set; }
        public bool RestringeAcesso { get; set; } = false;
        public ICollection<MensajeRolAutorizacionDto> Roles { get; set; }
        public ICollection<MensajeEntidadDto> Entidades { get; set; }
    }
}
