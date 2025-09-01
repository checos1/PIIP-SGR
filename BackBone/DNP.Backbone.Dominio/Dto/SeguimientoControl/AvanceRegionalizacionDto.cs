using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    public class AvanceRegionalizacionDto
    {

        public int? ProyectoId { get; set; }
        public string Bpin { get; set; }
        //public int? ProductoId { get; set; }
        //public List<ListaPeriodosActivos> ListaPeriodosActivos { get; set; }
        public List<FuentesRegionaliza> Fuentes { get; set; }
    }

    public class ListaPeriodosActivos
    {
        public int? Vigencia { get; set; }
        public string Mes { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
    }

    public class FuentesRegionaliza
    {
        public int? FuenteId { get; set; }
        public string NombreFuente { get; set; }
        public List<ObjetivosRegionaliza> Objetivos { get; set; }
    }

    public class ObjetivosRegionaliza
    {
        public int? NumeroObjetivo { get; set; }
        public int? ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public string ObjetivoEspecificoCorto { get; set; }
        public List<ProductosRegionaliza> Productos { get; set; }
    }

    public class ProductosRegionaliza
    {
        public int? NumeroProducto { get; set; }
        //public int? ProductCatalogId { get; set; }
        public int? ProductoId { get; set; }
        public string NombreProducto { get; set; }
        //public string Etapa { get; set; }
        //public double? CostoProducto { get; set; }
        //public double? CostoUnitario { get; set; }
        //public string DuracionPromedio { get; set; }
        //public string FechaInicio { get; set; }
        //public string FechaFin { get; set; }
        //public string NombreProductoCorto { get; set; }
        public List<LocalizacionesRegionaliza> Localizaciones { get; set; }
    }

    public class LocalizacionesRegionaliza
    {
        public int? LocalizacionId { get; set; }
        //public int? Departamento { get; set; }
        //public string Municipio { get; set; }
        //public string TipoAgrupacion { get; set; }
        //public string Agrupacion { get; set; }
        public List<RecursosPeriodosActivos> RecursosPeriodosActivos { get; set; }
        public List<MetasPeriodosActivos> MetasPeriodosActivos { get; set; }
        //public List<ResumenRecursosMetas> ResumenRecursosMetas { get; set; }
    }

    public class RecursosPeriodosActivos
    {
        public int? Vigencia { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        //public string Mes { get; set; }
        //public DateTime? FechaDesde { get; set; }
        //public DateTime? FechaHasta { get; set; }
        //public double? Inicial { get; set; }
        //public double? ValorVigente { get; set; }
        public string VigenteDelMes { get; set; }
        public string Compromisos { get; set; }
        public string Obligaciones { get; set; }
        public string Pagos { get; set; }
        public string ObservacionRecurso { get; set; }

    }

    public class MetasPeriodosActivos
    {
        public int? Vigencia { get; set; }
        public int? PeriodoProyectoId { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        //public string Mes { get; set; }
        //public DateTime? FechaDesde { get; set; }
        //public DateTime? FechaHasta { get; set; }
        public string ObservacionMeta { get; set; }
        public double? AcumuladoMesAnterior { get; set; }
        public string AvanceMes { get; set; }
    }

    public class ResumenRecursosMetas
    {
        public int? Vigencia { get; set; }
        public double? TotalVigenteFirme { get; set; }
        public double? TotalVigente { get; set; }
        public double? TotaCompromisos { get; set; }
        public double? TotalObligaciones { get; set; }
        public double? TotalPagos { get; set; }
        public double? TotalAvanceAcumulado { get; set; }
        public List<DetalleVigencia> DetalleVigencia { get; set; }
    }

    public class DetalleVigencia
    {
        public int? PeriodosPeriodicidadId { get; set; }
        public string Mes { get; set; }
        public double? ValorVigenteFirme { get; set; }
        public double? ValorVigenteMes { get; set; }
        public double? ValorCompromisos { get; set; }
        public double? ValorObligaciones { get; set; }
        public double? ValorPagos { get; set; }
        public string AvanceMes { get; set; }
        public string ObservacionRecurso { get; set; }
        public string ObservacionMeta { get; set; }

    }
}
