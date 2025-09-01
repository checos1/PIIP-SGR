using System;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class CalendarioExcepcionDto
    {
        public string CodigoEntidad { get; set; }
        public string Entidad { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int CalendarioId { get; set; }
        public int CalendarioExcepcionEntidadId { get; set; }
    }
}