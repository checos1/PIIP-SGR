namespace DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad
{
    public class VigenciasFuentesNoSGRDto
    {
        public int VigenciaFuente { get; set; }
        public string Fuente { get; set; }
        public int TipoCofinanciadorId { get; set; }
        public string CodigoCofinanciador { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
    }
}
