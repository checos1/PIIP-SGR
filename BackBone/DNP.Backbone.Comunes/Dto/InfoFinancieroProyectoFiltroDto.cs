using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class InfoFinancieroProyectoFiltroDto
    {
        public ParametrosDto ParametrosDto { get; set; }
        public List<int> ProyectosIds { get; set; }
        public string AvanceFinanciero { get; set; }
        public string AvanceFisico { get; set; }
        public string AvanceProyecto { get; set; }
        public string Duracion { get; set; }
        public string PeriodoEjecucion { get; set; }
    }
}
