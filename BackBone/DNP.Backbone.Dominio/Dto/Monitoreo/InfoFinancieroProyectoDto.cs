using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Monitoreo
{
    [ExcludeFromCodeCoverage]
    public class InfoFinancieroProyectoDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public string AvanceFinanciero { get; set; }
        public string AvanceFisico { get; set; }
        public string AvanceProyecto { get; set; }
        public string Duracion { get; set; }
        public string PeriodoEjecucion { get; set; }
        public bool Activo { get; set; }
    }
}
