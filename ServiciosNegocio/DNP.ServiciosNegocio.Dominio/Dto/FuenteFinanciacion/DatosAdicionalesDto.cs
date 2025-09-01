using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    public class DatosAdicionalesDto
    {
        public int proyectoId { get; set; }
        public int fuenteId { get; set; }
        public string tipoCofinanciadorId { get; set; }
        public string cofinanciador { get; set; }
        public int fuenteCofinanciadorId { get; set; }
        public string codigoCofinanciador { get; set; }
    }

    public class DatosAdicionalesResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }

}
