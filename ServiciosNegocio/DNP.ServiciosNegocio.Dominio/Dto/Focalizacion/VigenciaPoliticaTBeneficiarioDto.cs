using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class VigenciaPoliticaTBeneficiarioDto
    {
        public int? Vigencia { get; set; }
        public List<LocalizacionPTBDto> Localizacion { get; set; }
    }
}
