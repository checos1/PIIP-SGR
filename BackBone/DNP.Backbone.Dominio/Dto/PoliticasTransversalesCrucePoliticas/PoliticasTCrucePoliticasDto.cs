using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas
{
    public class PoliticasTCrucePoliticasDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
       public int FuenteId { get; set; }
        public List<PoliticaPrincipal> PoliticaPrincipal { get; set; }
    }

    public class PoliticaPrincipal
    {
        public int PoliticaId { get; set; }
        public string Politica { get; set; }
        public List<Localizaciones> Localizaciones { get; set; }
    }

    public class Localizaciones
    {
        public int LocalizacionId { get; set; }
        public string Localizacion { get; set; }
        public List<RelacionPoliticas> RelacionPoliticas { get; set; }
    }
    public class RelacionPoliticas
    {
       // public int OrdenId { get; set; }
        public string TituloPoliticaPrincipal { get; set; }
        public int PoliticaDependienteId { get; set; }
        public string PoliticaDependiente { get; set; }
        public List<CrucePoliticasVigencias> CrucePoliticasVigencias { get; set; }
    }

    public class CrucePoliticasVigencias
    {
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public decimal ValorPoliticaPrincipal { get; set; }
        public decimal ValorPoliticaDependiente { get; set; }
        public decimal ValorCruceDependientePrincipal { get; set; }
    }
}
