using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class LocalizacionPoliticaTBeneficiarioDto
    {       
        public List<VigenciaPoliticaTBeneficiarioDto> Vigencias { get; set; }
    }
}
