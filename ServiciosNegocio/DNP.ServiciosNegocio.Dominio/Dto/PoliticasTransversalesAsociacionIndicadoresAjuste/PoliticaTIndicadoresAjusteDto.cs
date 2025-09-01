using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadoresAjuste
{
    public class PoliticaTIndicadoresAjusteDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public List<Politicas> Politicas { get; set; }
    }
    public class IndicadoresPolitica
    {
        public int IndicadorId { get; set; }
        public string Indicador { get; set; }
    }

    public class Dimensione
    {
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public List<IndicadoresPolitica> IndicadoresPolitica { get; set; }
    }

    public class Politicas
    {
        public int PoliticaId { get; set; }
        public string Politica { get; set; }
        public List<Dimensione> Dimensiones { get; set; }
    }
}
