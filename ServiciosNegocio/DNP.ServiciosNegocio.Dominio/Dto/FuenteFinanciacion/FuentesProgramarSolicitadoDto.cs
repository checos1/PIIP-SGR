using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuentesProgramarSolicitadoDto
    {
        public string bpin{ get; set; }
        public string etapa { get; set; }
        public int fuenteId { get; set; }
        public string tipoCofinanciadorId { get; set; }
        public string tipoCofinanciador { get; set; }
        public string financiador { get; set; }
        public string recurso { get; set; }
        public string vigencias { get; set; }
    }

}
