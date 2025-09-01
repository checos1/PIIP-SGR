namespace DNP.Backbone.Dominio.Dto.Inbox
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ProyectoEntidadDto
    {
        public int SectorId { get; set; }
        public string SectorNombre { get; set; }
        public int EntidadId { get; set; }
        public string EntidadNombre { get; set; }
        public int ProyectoId { get; set; }
        public string ProyectoNombre { get; set; }
        public string CodigoBpin { get; set; }
    }
}
