using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class CalendarioProgramacionDto
    {
        public string NombrePaso { get; set; }
        public string NombreSeccion { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int MacroprocesoSeccionId { get; set; }
        public int SeccionId { get; set; }
        public int AccionesFlujosId { get; set; }
        public int FaseId { get; set; }
        public int CalendarioId { get; set; }
        public List<CalendarioExcepcionDto> Excepciones { get; set; }
    }
}
