using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class AjustesRegionalizaFuentesDto
    {
        public int? ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<AjustesRegionalizaFuentesFuentesDto> Fuentes { get; set; }
        public List<AjustesRegionalizaFuentesRegionalizacionDto> Regionalizacion { get; set; }
    }

    public class AjustesRegionalizaFuentesFuentesDto
    {      
        public int? Vigencia { get; set; }
        public List<AgrupacionesFuenteDto> AgrupacionFuente { get; set; }
    }

    public class AgrupacionesFuenteDto
    {
        public string Agrupacion { get; set; }
        public decimal? ValorInicial { get; set; }
        public decimal? ValorVigenteAjuste { get; set; }
    }

    public class AjustesRegionalizaFuentesRegionalizacionDto
    {
        public int? Vigencia { get; set; }
        public List<RegionalizacionAgrupacionesFuenteDto> AgrupacionesFuente { get; set; }
    }

    public class RegionalizacionAgrupacionesFuenteDto
    {
        public int? FuenteId { get; set; }
        public int? ProgramacionFuenteId { get; set; }
        public string AgrupacionFuente { get; set; }
        public List<RegionalizacionLocalizacionDto> Localizacion { get; set; }
    }

    public class RegionalizacionLocalizacionDto
    {
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public int? DepartamentoId { get; set; }
        public string Departamento { get; set; }
        public int? MunicipioId { get; set; }
        public string Municipio { get; set; }
        public int? TipoAgrupacionId { get; set; }
        public string TipoAgrupacion { get; set; }
        public int? AgrupacionId { get; set; }
        public string Agrupacion { get; set; }
        public int? RegionalizacionRecursosIdSolicitado { get; set; }
        public int? RegionalizacionRecursosIdInicial { get; set; }
        public int? RegionalizacionRecursosIdVigente { get; set; }
        public decimal? ValorSolicitado { get; set; }
        public decimal? ValorInicial { get; set; }
        public decimal? ValorVigente { get; set; }
        public decimal? ValorSolicitadoFirme { get; set; }
        public decimal? ValorInicialFirme { get; set; }
        public decimal? ValorVigenteFirme { get; set; }
        public decimal? ValorInicialAjuste { get; set; }
        public decimal? ValorAjuste { get; set; }
    }
}
