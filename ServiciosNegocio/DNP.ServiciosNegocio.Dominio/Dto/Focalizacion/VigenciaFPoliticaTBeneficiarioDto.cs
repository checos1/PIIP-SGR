using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class VigenciaFPoliticaTBeneficiarioDto
    {
        public int? Vigencia { get; set; }
        public decimal? Total_PGN_Nacion { get; set; }
        public decimal? Total_PGN_Propios { get; set; }
        public decimal? Total_SGR { get; set; }
        public decimal? Total_Territorio { get; set; }
        public decimal? Total_Empresa { get; set; }
        public decimal? Total_Privado { get; set; }
        public List<LocalizacioDto> Localizaciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LocalizacioDto
    {
        public int? LocalizacionId { get; set; }
        public string Ubicacion { get; set; }
        public int? Beneficiarios { get; set; }
        public int? ResourceTypeId1 { get; set; }
        public int? ResourceGroupId1 { get; set; }
        public int? LTipoCuantificacionFocalizacionId1 { get; set; }
        public decimal? PGN_NACION { get; set; }
        public int? ResourceTypeId2 { get; set; }
        public int? ResourceGroupId2 { get; set; }
        public int? LTipoCuantificacionFocalizacionId2 { get; set; }
        public decimal? PGN_PROPIOS { get; set; }
        public int? ResourceTypeId3 { get; set; }
        public int? ResourceGroupId3 { get; set; }
        public int? LTipoCuantificacionFocalizacionId3 { get; set; }
        public decimal? SGR { get; set; }
        public int? ResourceTypeId4 { get; set; }
        public int? ResourceGroupId4 { get; set; }
        public int? LTipoCuantificacionFocalizacionId4 { get; set; }
        public decimal? Territorial { get; set; }
        public int? ResourceTypeId5 { get; set; }
        public int? ResourceGroupId5 { get; set; }
        public int? LTipoCuantificacionFocalizacionId5 { get; set; }
        public decimal? EMPRESA { get; set; }
        public int? ResourceTypeId6 { get; set; }
        public int? ResourceGroupId6 { get; set; }
        public int? LTipoCuantificacionFocalizacionId6 { get; set; }
        public decimal? PRIVADO { get; set; }
    }
}
