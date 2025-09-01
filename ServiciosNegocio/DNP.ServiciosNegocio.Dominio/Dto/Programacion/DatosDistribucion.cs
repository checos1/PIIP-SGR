namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class DatosDistribucion
    {
        public int LocalizacionId { get; set; }
        public decimal ValorDistribuidoNacion { get; set; }
        public decimal ValorDistribuidoPropios { get; set; }
        public decimal ValorRecursosAprobadosNacion { get; set; }
        public decimal ValorRecursosAprobadosPropios { get; set; }
    }
}