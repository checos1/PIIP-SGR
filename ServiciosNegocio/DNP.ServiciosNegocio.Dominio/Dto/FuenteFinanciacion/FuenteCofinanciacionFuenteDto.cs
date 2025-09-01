namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuenteCofinanciacionFuenteDto
    {
        public int? FuenteId { get; set; }
        public string Fuente { get; set; }
        public int? TipoEntidadId { get; set; }
        public string TipoEntidad { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? TipoRecursoId { get; set; }
        public string TipoRecurso { get; set; }
        public bool? Seleccionado { get; set; }
    }
}
