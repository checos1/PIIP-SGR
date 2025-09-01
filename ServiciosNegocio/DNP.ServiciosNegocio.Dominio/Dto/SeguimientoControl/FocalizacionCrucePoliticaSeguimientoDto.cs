using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl
{
    public class FocalizacionCrucePoliticaSeguimientoDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<DatosFocalizac> DatosFocaliza { get; set; }
    }
    public class DatosFocalizac
    {
        public int FuenteId { get; set; }
        public int PoliticaPrincipalId { get; set; }
        public int LocalizacionId { get; set; }
        public int PoliticaDependienteId { get; set; }
        public int DimensionId { get; set; }
        public List<Recursosc> Recursos { get; set; }
        public List<Beneficiariosc> Beneficiarios { get; set; }
    }

    public class Recursosc
    {
        public int PeriodoProyectoId { get; set; }
        public int PeriodosPeriodicidadId { get; set; }
        public double Compromisos { get; set; }
        public double Obligaciones { get; set; }
        public double Pagos { get; set; }
        public string Observacion { get; set; }

    }
    public class Beneficiariosc
    {
        public int PeriodoProyectoId { get; set; }
        public int PeriodosPeriodicidadId { get; set; }
        public int AvanceBeneficiarios { get; set; }
        public string Observacion { get; set; }
    }
}
