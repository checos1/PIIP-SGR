using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales
{
    public class ValidacionOCADPazDto
    {
        public string Seccion { get; set; }
        public string Capitulo { get; set; }
        public string Error { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
    }
}
