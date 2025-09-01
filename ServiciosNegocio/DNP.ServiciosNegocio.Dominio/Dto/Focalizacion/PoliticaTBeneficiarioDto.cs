using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class PoliticaTBeneficiarioDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<LocalizacionPoliticaTBeneficiarioDto> Beneficiarios { get; set; }
        public List<FocalizacionPoliticaTBeneficiarioDto> Focalizacion_Beneficiarios_y_Recursos { get; set; }
    }
}
