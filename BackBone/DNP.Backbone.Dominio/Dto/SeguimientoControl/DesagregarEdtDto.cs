using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]

    public class ConsultaObjetivosProyecto
    {
        public string BPIN { get; set; }
        public string UsuarioDNP { get; set; }
        public string TokenAutorizacion { get; set; }
 
        public ConsultaObjetivosProyecto(string bpin)
        {
            BPIN = bpin;
        }
    }

    public class VigenciaEntregable
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
        public decimal Valor { get; set; }
        public decimal ValorBase { get; set; }
        public string Observaciones { get; set; }
    }

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
        public decimal Valor { get; set; }
        public decimal ValorBase { get; set; }
        public string Observaciones { get; set; }
    }
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
    public class PeriodosPeriodicidadDto
    {
        public int Id { get; set; }
        public string Mes { get; set; }
    }

    public class CalendarioPeriodoDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
    }

    public class DesagregarEdtNivelesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Objetivo> Objetivos { get; set; }
    }

    public class CatalogoEntregable
    {
        public int DeliverableCatalogId { get; set; }
        public string Nivel { get; set; }
        public string NombreEntregable { get; set; }
        public int parentId { get; set; }
        public int EntregableIdPrimerNivel { get; set; }
        public int ProductoId { get; set; }
        public int ActividadId { get; set; }
        public int ClassificacionId { get; set; }
        public int ClasificacionPadreId { get; set; }
        public int UnidadMedidaId { get; set; }
        public int DeliverableCatalogPadreId { get; set; }
        public int? Padreid { get; set; }
        public string UnidadMedida { get; set; }
        public bool Selected { get; set; }
    }

    public class RegistroModel
    {
        public string Tipo { get; set; }
        public List<RegistroEntregable> NivelesNuevos { get; set; }
    }

    public class DescendenciaModel
    {
        public int ActividadSeguimientoId { get; set; }
    }

    public class RegistroEntregable
    {
        public int? DeliverableCatalogId { get; set; }
        public string Nivel { get; set; }
        public string NombreEntregable { get; set; }
        public int ProductoId { get; set; }
        public int? Padreid { get; set; }
        public int ActividadId { get; set; }
        public int? UnidadMedidaId { get; set; }
        public int ActividadSeguimientoId { get; set; }
    }

    public class EntregablesNivel1 : SeguimientoEntregable
    {
        public int? DeliverableCatalogId { get; set; }
        public int IndexObjetivo { get; set; }
        public int IndexProducto { get; set; }
        public int IndexNivel1 { get; set; }
        public bool Deliverable { get; set; }
        public bool Nivel2 { get; set; }
        public bool Nivel3 { get; set; }
        public double Costo { get; set; }
        public string NumeroEntregableNivel1 { get; set; }
        public List<RequisitosObligatorios> RequisitosObligatorios { get; set; }
        public List<CatalogoEntregable> CatalogoEntregables { get; set; }
        public List<SeguimientoEntregable> SeguimientoEntregables { get; set; }
        public List<SeguimientoEntregable> NivelesRegistrados { get; set; }

        public EntregablesNivel1()
        {
            RequisitosObligatorios = new List<RequisitosObligatorios>();
            SeguimientoEntregables = new List<SeguimientoEntregable>();
            NivelesRegistrados = new List<SeguimientoEntregable>();
        }
    }

    public class RequisitosObligatorios
    {
        public string Nombre { get; set; }
        public string TipoEntregable { get; set; }
        public bool RequisitoCumple { get; set; }
        public int Position { get; set; }
    }

    public class Objetivo
    {
        public string IdCompuesto { get; set; }
        public int ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string ObjetivoEspecificoCorto { get; set; }
        public string NumeroObjetivo { get; set; }
        public List<Producto> Productos { get; set; }
    }

    public class Producto
    {
        public string IdCompuesto { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string NombreProductoCorto { get; set; }
        public string IndicadorPrincipal { get; set; }
        public int IndicadorId { get; set; }
        public string UnidadMedidaProducto { get; set; }
        public double Cantidad { get; set; }
        public double CostoProducto { get; set; }
        public string EsAcumulativo { get; set; }
        public string NumeroProducto { get; set; }
        public List<EntregablesNivel1> EntregablesNivel1 { get; set; }
        public List<EntregablesNivel1> Actividades { get; set; }
        public List<RequisitosObligatorios> RequisitosObligatorios { get; set; }
        
        public Producto()
        {
            RequisitosObligatorios = new List<RequisitosObligatorios>();
            Actividades = new List<EntregablesNivel1>();
            EntregablesNivel1 = new List<EntregablesNivel1>();
        }
    }

    public class SeguimientoEntregable: ProgramarActividadesDto
    {
        public string IdCompuesto { get; set; }
        public string NivelEntregable { get; set; }
        public int? EntregableCatalogId { get; set; }
        public string NombreEntregable { get; set; }
        public string NombreEntregableCorto { get; set; }
        public int? PadreId { get; set; }
        public string CodigoCatalogoEntregable { get; set; }
        public int? SeguimientoEntregablePadreId { get; set; }
        public string TipoEntregable { get; set; }
        public string Consecutivo { get; set; }
        public double? CostoTotal { get; set; }
        public double? CostoUnitario { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Unidad { get; set; }
        public bool ContieneSiguienteNivel { get; set; }
        public string NumeroEntregableNivel2 { get; set; }
        public string NumeroEntregableNivel3 { get; set; }
        public string NumeroActividad { get; set; }

        public List<SeguimientoEntregable> Hijos { get; set; }
        public List<DescendenciaModel> Descendencia { get; set; }
        public SeguimientoEntregable()
        {
            Hijos = new List<SeguimientoEntregable>();
        }
    }

    public class ReponseHttp
    {
        public bool Status { get; set; }
        public string Message { get; set; }

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
