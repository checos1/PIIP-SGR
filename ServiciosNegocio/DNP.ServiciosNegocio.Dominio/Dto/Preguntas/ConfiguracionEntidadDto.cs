namespace DNP.ServiciosNegocio.Dominio.Dto.Preguntas
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ConfiguracionEntidadDto
    {
        public int? ProyectoId { get; set; }
        public int? FaseId { get; set; }
        public string Fase { get; set; }
        public string AplicaTecnico { get; set; }
    }
}
