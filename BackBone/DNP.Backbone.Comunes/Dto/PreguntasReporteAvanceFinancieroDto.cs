using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class PreguntasReporteAvanceFinancieroDto
    {
        public Guid IdInstancia { get; set; }
        public int IdProyecto { get; set; }
        public string IdUsuarioDNP { get; set; }
        public Guid IdNivel { get; set; }
        public int PoliticaId { get; set; }
        public string Respuesta { get; set; }
        public string ObservacionPregunta { get; set; }
        public int EnvioPoliticaSubDireccionIdAgrupa { get; set; }
        public int PreguntaId { get; set; }
    }
}
