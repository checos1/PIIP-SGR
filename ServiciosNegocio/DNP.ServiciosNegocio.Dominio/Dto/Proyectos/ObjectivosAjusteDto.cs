using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ObjectivosAjusteDto
    {
        public int Proyectoid { get; set; }
        public List<ObjetivoDto> Objetivos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EntregablesActividadesDto
    {
        public int EntregableActividadId { get; set; }
        public string EntregableActividad { get; set; }
        public String CostoAjusteProyecto { get; set; }
        public String CostoFirmeProyecto { get; set; }
        public String CostoMGAProyecto { get; set; }
        public int Vigencia { get; set; }
        public int Firme { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaAjusteDto
    {
        public int Vigencia { get; set; }
        public List<EntregablesActividadesDto> EntregablesActividades { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaEntregableDto
    {
        public int Vigencia { get; set; }
        public int PeriodoProyectoId { get; set; }
        public decimal TotalVigencia { get; set; }
        public decimal TotalVigenciaAnterior { get; set; }
        public decimal TotalVigenciaCE { get; set; }
        public decimal TotalVigenciaCEAnterior { get; set; }
        public double TotalVigenciaCP { get; set; }
        public double TotalVigenciaCPAnterior { get; set; }
        public double TotalVigenciaCFC { get; set; }
        public double TotalVigenciaCFCAnterior { get; set; }
        public decimal TotalBaseCE { get; set; }
        public double TotalBaseCP { get; set; }
        public double TotalBaseCFC { get; set; }
        public string UsuarioDNP { get; set; }
        public List<DetalleActividadProgramacionSeguimientoPeriodosValoresDto> ActividadProgramacionSeguimientoPeriodosValores { get; set; }
        public List<PeriodosPeriodicidadDto> PeriodosPeriodicidad { get; set; }
        public List<ProgramacionSeguimientoPeriodosValoresDto> ProgramacionSeguimientoPeriodosValores { get; set; }
        public List<DetalleReporteProgramacionSeguimientoPeriodosValoresCEDto> ProgramacionSeguimientoPeriodosValoresCantidadEjecutada { get; set; }
        public List<DetalleReporteProgramacionSeguimientoPeriodosValoresCPDto> ProgramacionSeguimientoPeriodosValoresCostoPresupuestal { get; set; }
        public List<DetalleReporteProgramacionSeguimientoPeriodosValoresCFCDto> ProgramacionSeguimientoPeriodosValoresCostoFlujoCaja { get; set; }
        public List<ProgramacionSeguimientoPeriodosValoresDto> ProgramacionSeguimientoPeriodosValoresCE { get; set; }
        public List<ProgramacionSeguimientoPeriodosValoresDto> ProgramacionSeguimientoPeriodosValoresCP { get; set; }
        public List<ProgramacionSeguimientoPeriodosValoresDto> ProgramacionSeguimientoPeriodosValoresCFC { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DetalleActividadProgramacionSeguimientoPeriodosValoresDto
    {
        public int Vigencia { get; set; }
        public int ProductoId { get; set; }
        public int ActividadId { get; set; }
        public int ObjetivoId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosValoresId { get; set; }
        public decimal? Valor { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DetalleReporteProgramacionSeguimientoPeriodosValoresCEDto
    {
        public int Vigencia { get; set; }
        public int ProductoId { get; set; }
        public int ActividadId { get; set; }
        public int ObjetivoId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosValoresId { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorBase { get; set; }
        public string Observaciones { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class DetalleReporteProgramacionSeguimientoPeriodosValoresCPDto
    {
        public int Vigencia { get; set; }
        public int ProductoId { get; set; }
        public int ActividadId { get; set; }
        public int ObjetivoId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosValoresId { get; set; }
        public double Valor { get; set; }
        public double ValorBase { get; set; }
        public string Observaciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DetalleReporteProgramacionSeguimientoPeriodosValoresCFCDto
    {
        public int Vigencia { get; set; }
        public int ProductoId { get; set; }
        public int ActividadId { get; set; }
        public int ObjetivoId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosValoresId { get; set; }
        public double Valor { get; set; }
        public double ValorBase { get; set; }
        public string Observaciones { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProgramacionSeguimientoPeriodosValoresDto
    {
        public int IdPeriodosPeriodicidad { get; set; }
        public string Mes { get; set; }
        public int Vigencia { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosId { get; set; }
        public int? ActividadProgramacionSeguimientoPeriodosValoresId { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAnterior { get; set; }
        public bool HabilitaEditar { get; set; }
        public double ValorCantidad { get; set; }
        public decimal ValorCantidadEjecutada { get; set; }
        public decimal ValorCantidadEjecutadaAnterior { get; set; }
        public double ValorCostoPresupuestal { get; set; }
        public double ValorCostoPresupuestalAnterior { get; set; }
        public double ValorCostoFlujoCaja { get; set; }
        public double ValorCostoFlujoCajaAnterior { get; set; }
        public string Observaciones { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class PeriodosPeriodicidadDto
    {
        public int Id { get; set; }
        public string Mes { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProductoAjusteDto
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public string Etapa { get; set; }
        public int AplicaEDT { get; set; }
        public String CostoFirme { get; set; }
        public String CostoAjuste { get; set; }
        public String CostoMGA { get; set; }
        public int ProyectoId { get; set; }
        public List<VigenciaAjusteDto> Vigencias { get; set; }
        public List<CatalogoEntregables> CatalogoEntregables { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ObjetivoDto
    {
        public int ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductoAjusteDto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CatalogoEntregables
    {
        public int EntregableId { get; set; }
        public string EntregableNombre { get; set; }
        public int ProductCatalogId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AgregarEntregable
    {
        public string nombre { get; set; }
        public string etapa { get; set; }
        public string productoId { get; set; }
        public string deliverable { get; set; }
        public string deliverableCatalogId { get; set; }
    }

    public class ReporteSeguimiento
    {
        public List<AvanceCantidadesDto> AvanceCantidades { get; set; }
        public List<CostoPeriodoDto> CostoPeriodo { get; set; }
        public string Igual { get; set; }
        public int ProyectoId { get; set; }
        public int ActividadId { get; set; }
        public int? ActividadSeguimientoId { get; set; }

    }
    public class ReporteIndicadorPoliticas
    {
        public List<ReporteIndicadoresDto> ReporteIndicadores { get; set; }
        public int ProyectoId { get; set; }
        public int PoliticaId { get; set; }
        public int FuenteId { get; set; }
        public int DimensionId { get; set; }
        public int LocalizacionId { get; set; }
    }
}
