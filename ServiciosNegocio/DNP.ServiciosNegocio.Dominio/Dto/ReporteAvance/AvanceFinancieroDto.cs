using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.ReporteAvance
{
    public class AvanceFinancieroDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Fuentes> Fuentes { get; set; }
    }

    public class Fuentes
    {
        public int FuenteId { get; set; }
        public List<RecursosVigentes> RecursosVigentes { get; set; }
        public List<RecursosPresupuestales> RecursosPresupuestales { get; set; }
    }

    public class RecursosVigentes
    {
        public int PeriodoproyectoId { get; set; }
        public int PeriodoPeriodicidadId { get; set; }
        public int Vigencia { get; set; }
        public double RecursosVigentesApropiacionInicial { get; set; }
        public double RecursosVigentesApropiacionVigente { get; set; }
        public double RecursosVigentesAcumuladoCompromisos { get; set; }
        public double RecursosVigentesAcumuladoObligaciones { get; set; }
        public double RecursosVigentesAcumuladoPagos { get; set; }
        public bool HabilitaApropiacionInicial { get; set; }
        public bool HabilitaApropiacionVigente { get; set; }
        public bool HabilitaAcumuladoCompromisos { get; set; }
        public bool HabilitaAcumuladoObligacion { get; set; }
        public bool HabilitaAcumuladoPagos { get; set; }
    }

    public class RecursosPresupuestales
    {
        public int PeriodoproyectoId { get; set; }
        public int PeriodoPeriodicidadId { get; set; }
        public int Vigencia { get; set; }
        public double ReservaPresupuestalApropiacionInicial { get; set; }
        public double ReservaPresupuestalApropiacionVigente { get; set; }
        public double ReservaPresupuestalAcumuladoCompromisos { get; set; }
        public double ReservaPresupuestalAcumuladoObligaciones { get; set; }
        public double ReservaPresupuestalAcumuladoPagos { get; set; }
        public bool HabilitaApropiacionInicial { get; set; }
        public bool HabilitaApropiacionVigente { get; set; }
        public bool HabilitaAcumuladoCompromisos { get; set; }
        public bool HabilitaAcumuladoObligacion { get; set; }
        public bool HabilitaAcumuladoPagos { get; set; }
    }
}
