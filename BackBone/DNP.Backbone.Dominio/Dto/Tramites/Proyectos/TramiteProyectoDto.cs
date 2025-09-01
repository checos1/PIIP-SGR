using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class TramiteProyectoDto
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public int EntidadId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public string Accion { get; set; }
        public bool Estado { get; set; }
        public string InstanciaId { get; set; }
        public string FlujoId { get; set; }
        public string TipoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public bool? EsConstante { get; set; }
        public int Constante { get; set; }
        public int? AnioBase { get; set; }
    }
}
