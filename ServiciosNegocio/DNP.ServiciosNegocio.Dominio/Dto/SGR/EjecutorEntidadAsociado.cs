using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR
{
    [ExcludeFromCodeCoverage]
    public class EjecutorEntidadAsociado
    {
        public int Id { get; set; }
        public int? EjecutorId { get; set; }
        public string NitEjecutor { get; set; }
        public string NombreEntidad { get; set; }
        public string TipoEntidad { get; set; }
    }
}
