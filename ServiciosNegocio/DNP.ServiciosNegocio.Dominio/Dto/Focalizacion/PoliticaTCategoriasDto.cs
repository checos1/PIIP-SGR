using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class PoliticaTCategoriasDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<PoliticaCategoriaDto> Politicas { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PoliticaCategoriaDto
    {
        public int? PoliticaId { get; set; }
        public string NombrePolitica { get; set; }
        public List<CategoriaDto> Categorias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CategoriaDto
    {
        public int? ProyectoFocalizacionId { get; set; }
        public int? CategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public List<VigenciaDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaDto
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
        public List<LocalizacionDto> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LocalizacionDto
    {
        public int? LocalizacionId { get; set; }
        public string Ubicacion { get; set; }
        public int? Beneficiarios { get; set; }
        public decimal? RecursosFocalizados { get; set; }
        public int? ResourceGroupPGNNacionId { get; set; }
        public int? ResourceTypePGNNacionId { get; set; }
        public decimal? PGNNacion { get; set; }
        public int? ResourceGroupPGNPropiosId { get; set; }
        public int? ResourceTypePGNPropiosId { get; set; }
        public decimal? PGNPropios { get; set; }
        public int? ResourceGroupSGRId { get; set; }
        public int? ResourceTypeSGRId { get; set; }
        public decimal? SGR { get; set; }
        public int? ResourceGroupTERRITORIOId { get; set; }
        public int? ResourceTypeTERRITORIOId { get; set; }
        public decimal? TERRITORIO { get; set; }
        public int? ResourceGroupEMPRESAId { get; set; }
        public int? ResourceTypeEMPRESAId { get; set; }
        public decimal? EMPRESA { get; set; }
        public int? ResourceGroupPRIVADOSId { get; set; }
        public int? ResourceTypePRIVADOSId { get; set; }
        public decimal? PRIVADOS { get; set; }
    }
}
