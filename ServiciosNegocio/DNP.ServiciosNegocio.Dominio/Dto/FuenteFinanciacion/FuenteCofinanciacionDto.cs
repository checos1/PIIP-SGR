using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class FuenteCofinanciacionDto
    {
        public int? CofinanciadorId { get; set; }
        public int? TipoCofinanciadorId { get; set; }
        public string TipoCofinanciador { get; set; }
        public string Cofinanciador { get; set; }
        public List<FuenteCofinanciacionFuenteDto> Fuentes { get; set; }
    }
}
