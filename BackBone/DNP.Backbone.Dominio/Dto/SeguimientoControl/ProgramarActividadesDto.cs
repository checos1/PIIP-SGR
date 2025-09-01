using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    public class ProgramarActividadesDto
    {
        public int? ActividadId { get; set; }
        public int? SeguimientoEntregableId { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
        public int? PredecesoraId { get; set; }
        public int? SeguimientoEntregablePredecesoraId { get; set; }
        public int ProyectoId { get; set; }
        public string Tipo { get; set; }
        public string Bpin { get; set; }
        public string TipoSigla { get; set; }
        public double CostoTotal { get; set; }
        public double CostoUnitario { get; set; }
        public int? CantidadTotal { get; set; }
        public float? DuracionOptimista { get; set; }
        public float? DuracionPesimista { get; set; }
        public float? DuracionProbable { get; set; }
        public float? DuracionPromedio { get; set; }
        public string UnidadMedida { get; set; }
        public int? UnidadMedidaId { get; set; }
        public string NombreActividad { get; set; }
        public string NombrePredecesora { get; set; }
        public int? PosPosicion { get; set; }
        public int? Adelanto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool HabilitaEditar { get; set; }
        public decimal TotalVigencia { get; set; }
        public decimal TotalVigenciaAnterior { get; set; }
        public List<VigenciaEntregable> Vigencias { get; set; }
        public List<AvanceCantidadesDto> AvanceCantidades { get; set; }
        public List<CostoPeriodoDto> CostoPeriodo { get; set; }
    }

    public class AvanceCantidadesDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
        public decimal CantidadProgramadaVigencia { get; set; }
        public decimal CantidadAcumuladaMeses { get; set; }
        public decimal CantidadEjecutadaMes { get; set; }
        public decimal CantidadEjecutadaMesAnterior { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAnterior { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
    }

    public class CostoPeriodoDto
    {
        public int CalendarioPeriodoId { get; set; }
        public int? FaseId { get; set; }
        public int? Vigencia { get; set; }
        public int? PeriodosPeriodicidadId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Mes { get; set; }
        public double CostoProgramadoVigencia { get; set; }
        public double CostoAcumuladoMeses { get; set; }
        public double CostoEjecutadoMes { get; set; }
        public double CostoEjecutadoMesAnterior { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesAnterior { get; set; }
        public string TipoCosto { get; set; }
        public int? ActividadProgramacionSeguimientoId { get; set; }
    }


    public class EditarActividadDto
    {
        public ProgramarActividadesDto Actividad { get; set; }

    }

}
