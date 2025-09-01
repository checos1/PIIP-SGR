using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class AjustesCuantificacionBeneficiarioDto
    {
        public string BPIN { get; set; }
        public int? PoblacionAfectada { get; set; }
        public int? PoblacionObjetivo { get; set; }
        public decimal? ValorTotalProyectoFirme { get; set; }
        public decimal? ValorTotalProyectoAjuste { get; set; }
        public List<VigenciasAjustesCuantificacionBeneficiarioDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciasAjustesCuantificacionBeneficiarioDto
    {
        public int? Vigencia { get; set; }
        public decimal? TotalBeneficiariosProyectoFirme { get; set; }
        public decimal? TotalBeneficiariosProyectoAjuste { get; set; }
        public List<LocalizacionAjustesCuantificacionBeneficiarioDto> Localizacion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LocalizacionAjustesCuantificacionBeneficiarioDto
    {
        public int? LocalizacionId { get; set; }
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? AgrupacionId { get; set; }
        public string NombreAgrupacion { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public decimal? NumeroBeneficiariosVigenteFirme { get; set; }
        public decimal? NumeroBeneficiariosVigenteAjuste { get; set; }
        public decimal? NumeroBeneficiariosSolicitadoFirme { get; set; }
        public decimal? NumeroBeneficiariosSolicitadoAjuste { get; set; }
    }
}
