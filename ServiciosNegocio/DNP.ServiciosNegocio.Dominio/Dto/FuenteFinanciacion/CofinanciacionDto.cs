namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class CofinanciacionDto
    {
        public int? ProyectoCofinanciadorId { get; set; }
        public int? TipoCofinanciadorId { get; set; }        
        public string TipoCofinanciador { get; set; }
        public string CofinanciadorId { get; set; }
    }
}
