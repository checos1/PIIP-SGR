using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class ResumenFuenteFinanciacionDTO
    {
        public string BPIN { get; set; }
        public List<Etapa> Etapas { get; set; }
    }

    //public class Vigencia
    //{
    //    public int Vigencia { get; set; }
    //    public double SolicitadoGestionRecursos { get; set; }
    //    public int InicialesDecretoLiquidacion { get; set; }
    //    public double ValorActual { get; set; }
    //    public double ValorFirme { get; set; }
    //}

    public class Fuente
    {
        public string Etapa { get; set; }
        public int FuenteId { get; set; }
        public int TipoFinanciadorId { get; set; }
        public string TipoFinanciador { get; set; }
        public string Financiador { get; set; }
        public string Recurso { get; set; }
        public List<Vigencias> vigencia { get; set; }
    }

    public class Etapa
    {
        public string ETAPA { get; set; }
        public List<Fuente> Fuentes { get; set; }
    }
}
