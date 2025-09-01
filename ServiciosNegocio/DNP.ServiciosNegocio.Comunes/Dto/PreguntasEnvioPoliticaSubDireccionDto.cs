using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto
{
    public class PreguntasEnvioPoliticaSubDireccionDto
    {
        public Guid IdInstancia { get; set; }
        public int IdProyecto { get; set; }
        public string IdUsuarioDNP { get; set; }
        public Guid IdNivel { get; set; }
    }
}
