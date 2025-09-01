using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class LocalizacionPTBDto
    {
        public int? LocalizacionId { get; set; }
        public string Ubicacion { get; set; }
        public int? Beneficiarios { get; set; }
        public int? TipoValorId { get; set; }
    }
}
