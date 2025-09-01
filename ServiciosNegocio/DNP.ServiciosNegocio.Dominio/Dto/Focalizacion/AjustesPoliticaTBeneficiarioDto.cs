namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class AjustesPoliticaTBeneficiarioDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<BeneficiarioAjustesPoliticaTBeneficiarioDto> Beneficiarios { get; set; }
        public List<FocalizacionAjustesPoliticaTBeneficiarioDto> Focalizacion_Beneficiarios_y_Recursos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class BeneficiarioAjustesPoliticaTBeneficiarioDto
    {
        public List<VigenciaAjustesPoliticaTBeneficiarioDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaAjustesPoliticaTBeneficiarioDto
    {
        public int? Vigencia { get; set; }
        public List<LocalizacionAjustesPoliticaTBeneficiarioDto> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LocalizacionAjustesPoliticaTBeneficiarioDto
    {
        public int? LocalizacionId { get; set; }
        public string Ubicacion { get; set; }
        public int? BeneficiarioVigenteenFirme { get; set; }
        public int? BeneficiarioVigenteenAjuste { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class FocalizacionAjustesPoliticaTBeneficiarioDto
    {
        public int? PoliticaId { get; set; }
        public string Politica_Transversal { get; set; }
        public int? FocalizacionPoliticaId { get; set; }        
        public List<VigenciaFAjustesPoliticaTBeneficiarioDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaFAjustesPoliticaTBeneficiarioDto
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public decimal? Total_PGN_Nacion { get; set; }
        public decimal? Total_PGN_Propios { get; set; }
        public decimal? Total_SGR { get; set; }
        public decimal? Total_Territorio { get; set; }
        public decimal? Total_Empresa { get; set; }
        public decimal? Total_Privado { get; set; }
        public List<LocalizacioFAjustesPoliticaTBeneficiarioDto> Localizaciones { get; set; }
    }

    public class LocalizacioFAjustesPoliticaTBeneficiarioDto
    {
        public int? LocalizacionId { get; set; }
        public string Ubicacion { get; set; }
        public int? BeneficiariosenFirme { get; set; }
        public int? BeneficiariosenAjuste { get; set; }
        public decimal? PGN_NACIONenFirme { get; set; }
        public decimal? PGN_PROPIOSenFirme { get; set; }
        public decimal? SGRenFirme { get; set; }
        public decimal? TerritorialenFirme { get; set; }
        public decimal? EMPRESAenFirme { get; set; }
        public decimal? PRIVADOenFirme { get; set; }
        public int? ResourceTypeId1 { get; set; }
        public int? ResourceGroupId1 { get; set; }
        public int? LTipoCuantificacionFocalizacionId1 { get; set; }
        public decimal? PGN_NACIONenAjuste { get; set; }
        public int? ResourceTypeId2 { get; set; }
        public int? ResourceGroupId2 { get; set; }
        public int? LTipoCuantificacionFocalizacionId2 { get; set; }
        public decimal? PGN_PROPIOSenAjuste { get; set; }
        public int? ResourceTypeId3 { get; set; }
        public int? ResourceGroupId3 { get; set; }
        public int? LTipoCuantificacionFocalizacionId3 { get; set; }
        public decimal? SGRenAjuste { get; set; }
        public int? ResourceTypeId4 { get; set; }
        public int? ResourceGroupId4 { get; set; }
        public int? LTipoCuantificacionFocalizacionId4 { get; set; }
        public decimal? TERRITORIOenAjuste { get; set; }
        public int? ResourceTypeId5 { get; set; }
        public int? ResourceGroupId5 { get; set; }
        public int? LTipoCuantificacionFocalizacionId5 { get; set; }
        public decimal? EMPRESAenAjuste { get; set; }
        public int? ResourceTypeId6 { get; set; }
        public int? ResourceGroupId6 { get; set; }
        public int? LTipoCuantificacionFocalizacionId6 { get; set; }
        public decimal? PRIVADOenAjuste { get; set; }
    }
}
