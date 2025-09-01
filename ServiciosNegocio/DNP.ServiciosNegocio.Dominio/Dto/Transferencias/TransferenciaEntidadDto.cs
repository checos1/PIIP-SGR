namespace DNP.ServiciosNegocio.Dominio.Dto.Transferencias
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class TransferenciaEntidadDto
    {
        public decimal ProyectoId { get; set; }
        public int EntidadTransfiereId { get; set; }
        public string EntidadDestino { get; set; }
        public int EntidadDestinoId { get; set; }
        public string MensajeError { get; set; }
    }
}
