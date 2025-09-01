using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SeguimientoControl
{
    public class FocalizacionProgramacionSeguimientoDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<DatosFocaliza> DatosFocaliza { get; set; }
    }
    public class DatosFocaliza
    {
        public int PoliticaId { get; set; }
        public int FuenteId { get; set; }
        public int DimensionId { get; set; }
        public int ProductoId { get; set; }
        public int LocalizacionId { get; set; }
        public List<Recursos> Recursos { get; set; }
        public List<Beneficiarios> Beneficiarios { get; set; }

        public List<Metas> Metas { get; set; }
    }

    public class Recursos {
        public int PeriodoProyectoId { get; set; }
        public int PeriodosPeriodicidadId { get; set; }
        public double VigenteDelMes { get; set; }
        public double Compromisos { get; set; }
        public double Obligaciones { get; set; }
        public double Pagos { get; set; }
        public string Observacion { get; set; }

    }
    public class Metas
    {
        public int PeriodoProyectoId { get; set; } 
        public int PeriodosPeriodicidadId { get; set; }
        public double AvanceIndicadorPpalMes { get; set; }
        public double AvanceIndicadorSecMes { get; set; }
        public string Observacion { get; set; }
    }
    public class Beneficiarios
    {
        public int PeriodoProyectoId { get; set; }
        public int PeriodosPeriodicidadId { get; set; }
        public int AvanceBeneficiariosMes { get; set; }
        public string Observacion { get; set; }
    }

}
