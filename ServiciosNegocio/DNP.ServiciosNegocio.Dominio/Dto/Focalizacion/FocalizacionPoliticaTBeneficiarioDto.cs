using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class FocalizacionPoliticaTBeneficiarioDto
    {
        public int? PoliticaId { get; set; }
        public int? FocalizacionPoliticaId { get; set; }
        public string Politica_Transversal { get; set; }
        public List<VigenciaFPoliticaTBeneficiarioDto> Vigencias { get; set; }
    }
}
