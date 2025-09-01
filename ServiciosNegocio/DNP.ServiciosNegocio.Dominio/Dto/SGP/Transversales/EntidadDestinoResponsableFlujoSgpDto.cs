using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales
{
    [ExcludeFromCodeCoverage]
    public class EntidadDestinoResponsableFlujoSgpDto
    {
        public int? EntidadDestinoAccionId { get; set; }
        public string Dependencia { get; set; }
    }
}
