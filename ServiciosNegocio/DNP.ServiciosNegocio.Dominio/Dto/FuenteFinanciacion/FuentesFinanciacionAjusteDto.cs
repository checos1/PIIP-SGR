using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuentesFinanciacionAjusteDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<ValoresProyecto> ValoresProyecto { get; set; }
        public List<Vigencias> Vigencias { get; set; }
    }
    public class Fuentes
    {
        public int FuenteId { get; set; }
        public string Fuente { get; set; }
        public string Etapa { get; set; }
        public int PeriodoProyectoId { get; set; }
        public double RecursosSolicitados { get; set; }
        public double RecursosIniciales { get; set; }
        public double RecursosVigentes { get; set; }
        public double RecursosEnajuste { get; set; }
    }
    public class Vigencias
    {
        public int Vigencia { get; set; }
        public List<Fuentes> Fuentes { get; set; }
        public double SolicitadoGestionRecursos { get; set; }
        public int InicialesDecretoLiquidacion { get; set; }
        public double ValorActual { get; set; }
        public double ValorFirme { get; set; }
    }

    public class ValoresProyecto
    {
        public int TipoValorId { get; set; }
        public string TipoValor { get; set; }
        public double ValorTotalProyecto { get; set; }
        public double ValorEtapaPreInversion { get; set; }
        public double ValorEtapaInversion { get; set; }
        public double ValorEtapaOperacion { get; set; }
        public double ValorPGN { get; set; }
        public double ValorSGR { get; set; }
        public double ValorTerritorial { get; set; }
    }
}
