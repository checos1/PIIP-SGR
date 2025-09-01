using DNP.Backbone.Dominio.Dto.Focalizacion;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    public class DesagregarIndicadoresPoliticasDto
    {
        public string BPIN { get; set; }
        public List<PoliticasDto> Politicas { get; set; }
    }

    public class PoliticasDto
    {
        public int PoliticaId { get; set; }
        public string NombrePolitica { get; set; }
        public string NombrePoliticaCorto { get; set; }
        public int LocalizacionId { get; set; }
        public List<FuentesDto> Fuentes { get; set; }
    }

    public class FuentesDto
    {
        public int FuenteId { get; set; }
        public string NombreFuente { get; set; }
        public string NombreFuenteCorto { get; set; }
        public List<CategoriasDto> Categorias { get; set; }
    }

    public class CategoriasDto
    {
        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreCategoriaCorto { get; set; }
        public int SubCategoriaId { get; set; }
        public string NombreSubCategoria { get; set; }
        public string NombreSubCategoriaCorto { get; set; }
        public List<ReporteIndicadoresDto> ReporteCategorias { get; set; }
        public List<DetalleProgramacionIndicadoresDto> ProgramacionCategorias { get; set; }
        public List<IndicadoresPoliticasDto> Indicadores { get; set; }
        
    }

    public class LocalizacionesIndicadoresDto
    {
        public int LocalizacionId { get; set; }
        public string NombreDepartamento { get; set; }
        public string NombreMunicipio { get; set; }
        public string NombreAgrupacion { get; set; }
        public string NombreTipoAgrupacion { get; set; }
        public List<VigenciaIndicadoresDto> Vigencias { get; set; }
        public List<ReporteIndicadoresDto> ReporteIndicadores { get; set; }

    }

    public class IndicadoresPoliticasDto
    {
        public int IndicadorId { get; set; }
        public string NombreIndicador { get; set; }
        public string CodigoIndicador { get; set; }
        public string NombreIndicadorCorto { get; set; }
        public List<LocalizacionesIndicadoresDto> Localizaciones { get; set; }
        
    }

    public class VigenciaIndicadoresDto
    {
        public int Vigencia { get; set; }
        public int PeriodoProyectoId { get; set; }
        public double TotalVigencia { get; set; }
        public double TotalCompromisos { get; set; }
        public double TotalObligaciones { get; set; }
        public double TotalPagos { get; set; }
        public string UsuarioDNP { get; set; }
        public List<DetalleProgramacionIndicadoresDto> ProgramacionIndicadores { get; set; }
        public List<DetalleProgramacionVigenciaIndicadoresDto> ProgramacionVigenciaIndicadores { get; set; }
        public List<PeriodosPeriodicidadDto> PeriodosPeriodicidad { get; set; }
    }

    public class DetalleProgramacionIndicadoresDto
    {
        public int PoliticaId { get; set; }
        public int? FuenteId { get; set; }
        public int DimensionId { get; set; }
        public int IndicadorId { get; set; }
        public int? LocalizacionId { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? PeriodoPeriodicidadId { get; set; }
        public string ObservacionPeriodo { get; set; }
        public int? TipoValorId { get; set; }
        public string NombreTipoValor { get; set; }
        public double? Valor { get; set; }
        public string ObservacionValores { get; set; }
        public int Vigencia { get; set; }
    }
    public class DetalleProgramacionVigenciaIndicadoresDto
    {
        public int PoliticaId { get; set; }
        public int? FuenteId { get; set; }
        public int DimensionId { get; set; }
        public int IndicadorId { get; set; }
        public int? LocalizacionId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int? PeriodoPeriodicidadId { get; set; }
        public string ObservacionPeriodo { get; set; }
        public int? TipoValorId { get; set; }
        public string NombreTipoValor { get; set; }
        public double ValorVigente { get; set; }
        public double ValorCompromisos { get; set; }
        public double ValorObligaciones { get; set; }
        public double ValorPagos { get; set; }
        public string ObservacionValores { get; set; }
        public int Vigencia { get; set; }
        public string Mes { get; set; }
    }

    public class ReporteIndicadoresDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? SeguimientoPeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
        public double ValorVigente { get; set; }
        public double ValorVigenteAnterior { get; set; }
        public double ValorCompromisos { get; set; }
        public double ValorCompromisosAnterior { get; set; }
        public double ValorObligaciones { get; set; }
        public double ValorObligacionesAnterior { get; set; }
        public double ValorPagos { get; set; }
        public double ValorPagosAnterior { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAnterior { get; set; }
        public int? IndicadorId { get; set; }
        public int? PoliticaId { get; set; }
        public int? FuenteId { get; set; }
        public int? LocalizacionId { get; set; }
    }
}
