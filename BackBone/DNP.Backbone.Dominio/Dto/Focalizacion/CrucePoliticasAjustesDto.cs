using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Focalizacion
{
    public class CrucePoliticasAjustesDto
    {
        public int ProyectoId { get; set; }
        public string Bpin { get; set; }
        public int FuenteId { get; set; }
        public int PoliticaId { get; set; }
        public int LocalizacionId { get; set; }
        public int PoliticaDependienteId { get; set; }
        public int PeriodoProyectoId { get; set; }
        public int Vigencia { get; set; }
        public double ValorPoliticaPrincipal { get; set; }
        public double ValorCruceDependientePrincipal { get; set; }
        public int PersonaPoliticaPrincipal { get; set; }
        public int PersonaCruce { get; set; }
        public int DimensionId { get; set; }
    }
}
