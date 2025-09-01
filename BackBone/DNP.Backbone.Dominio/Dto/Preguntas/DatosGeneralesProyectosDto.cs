namespace DNP.Backbone.Dominio.Dto.Preguntas
{
    public class DatosGeneralesProyectosDto
    {
        public int? ProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public string BPIN { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? SectorId { get; set; }
        public string Sector { get; set; }
        public int? EstadoId { get; set; }
        public string Estado { get; set; }
        public string Horizonte { get; set; }
        public decimal Valor { get; set; }
    }
}
