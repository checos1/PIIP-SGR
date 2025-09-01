using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class AjustesPoliticaTCategoriasDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<PoliticaAjustesPoliticaCategoriaDto> Politicas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticaAjustesPoliticaCategoriaDto
    {
        public int? PoliticaId { get; set; }
        public string NombrePolitica { get; set; }
        public List<CategoriaAjustesPoliticaCategoriaDto> Categorias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CategoriaAjustesPoliticaCategoriaDto
    {
        public int? ProyectoFocalizacionId { get; set; }
        public int? CategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public List<VigenciaAjustesPoliticaCategoriaDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaAjustesPoliticaCategoriaDto
    {
        public int? PeriodoProyectoId { get; set; }
        public int? Vigencia { get; set; }
        public decimal? TotalSolicitado_PGN_Nacion { get; set; }
        public decimal? TotalSolicitado_PGN_Propios { get; set; }
        public decimal? TotalSolicitado_SGR { get; set; }
        public decimal? TotalSolicitado_Territorio { get; set; }
        public decimal? TotalSolicitado_Empresa { get; set; }
        public decimal? TotalSolicitado_Privado { get; set; }
        public decimal? TotalVigente_PGN_Nacion { get; set; }
        public decimal? TotalVigente_PGN_Propios { get; set; }
        public decimal? TotalVigente_SGR { get; set; }
        public decimal? TotalVigente_Territorio { get; set; }
        public decimal? TotalVigente_Empresa { get; set; }
        public decimal? TotalVigente_Privado { get; set; }
        public List<LocalizacionAjustesPoliticaCategoriaDto> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LocalizacionAjustesPoliticaCategoriaDto
    {
        public int? LocalizacionId { get; set; }
        public string Ubicacion { get; set; }
        public int? BeneficiariosVigenteFirme { get; set; }
        public int? BeneficiariosVigenteAjuste { get; set; }
        public decimal? RecursosFocalizadosVigenteFirme { get; set; }
        public decimal? RecursosFocalizadosSolicitadosFirme { get; set; }
        public decimal? RecursosFocalizadosVigenteAjuste { get; set; }
        public decimal? RecursosFocalizadosSolicitadosAjuste { get; set; }
        public decimal? PGNNacionVigenteFirme { get; set; }
        public decimal? PGNNacionSolicitadoFirme { get; set; }
        public decimal? PGNPropiosVigenteFirme { get; set; }
        public decimal? PGNPropiosSolicitadoFirme { get; set; }
        public decimal? SGRVigenteFirme { get; set; }
        public decimal? SGRSolicitadoFirme { get; set; }
        public decimal? TERRITORIOVigenteFirme { get; set; }
        public decimal? TERRITORIOSolicitadoFirme { get; set; }
        public decimal? EMPRESAVigenteFirme { get; set; }
        public decimal? EMPRESASolicitadoFirme { get; set; }
        public decimal? PRIVADOSVigenteFirme { get; set; }
        public decimal? PRIVADOSSolicitadoFirme { get; set; }
        public int? ResourceGroupPGNNacionId { get; set; }
        public int? ResourceTypePGNNacionId { get; set; }
        public decimal? PGNNacionVigenteAjuste { get; set; }
        public decimal? PGNNacionSolicitadoAjuste { get; set; }
        public int? ResourceGroupPGNPropiosId { get; set; }
        public int? ResourceTypePGNPropiosId { get; set; }
        public decimal? PGNPropiosVigenteAjuste { get; set; }
        public decimal? PGNPropiosSolicitadoAjuste { get; set; }
        public int? ResourceGroupSGRId { get; set; }
        public int? ResourceTypeSGRId { get; set; }
        public decimal? SGRVigenteAjuste { get; set; }
        public decimal? SGRSolicitadoAjuste { get; set; }
        public int? ResourceGroupTERRITORIOId { get; set; }
        public int? ResourceTypeTERRITORIOId { get; set; }
        public decimal? TERRITORIOVigenteAjuste { get; set; }
        public decimal? TERRITORIOSolicitadoAjuste { get; set; }
        public int? ResourceGroupEMPRESAId { get; set; }
        public int? ResourceTypeEMPRESAId { get; set; }
        public decimal? EMPRESAVigenteAjuste { get; set; }
        public decimal? EMPRESASolicitadoAjuste { get; set; }
        public int? ResourceGroupPRIVADOSId { get; set; }
        public int? ResourceTypePRIVADOSId { get; set; }
        public decimal? PRIVADOSVigenteAjuste { get; set; }
        public decimal? PRIVADOSSolicitadoAjuste { get; set; }
    }
}
