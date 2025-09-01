using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class EliminacionAsociacionDto
    {
        public string TramiteId { get; set; }
        public string ProyectoId { get; set; }
        public string InstanciaId { get; set; }
    }
}
