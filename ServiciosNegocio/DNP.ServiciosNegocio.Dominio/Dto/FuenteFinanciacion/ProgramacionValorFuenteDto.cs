using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class ProgramacionValorFuenteDto
    {
        public string bpin { get; set; }
        public string etapa { get; set; }
        public int fuenteId { get; set; }
        public List<ListaVigencias> valores { get; set; }
    }

}
