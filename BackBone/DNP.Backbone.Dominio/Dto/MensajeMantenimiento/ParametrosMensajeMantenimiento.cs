using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Filtros.MensajeMantenimiento;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.MensajeMantenimiento
{
    public class ParametrosMensajeMantenimiento
    {
        public ParametrosDto ParametrosDto { get; set; }
        public MensajeMantenimientoDto MensajeMantenimientoDto { get; set; }
        public MensajeMantenimientoFiltroDto FiltroDto { get; set; }
    }
}
