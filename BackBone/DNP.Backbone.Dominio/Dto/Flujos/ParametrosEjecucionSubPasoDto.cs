using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Flujos.Dominio.Dto.Flujos
{
    public class ParametrosEjecucionSubPasoDto
    {
        public Guid InstanciaId { get; set; }
        public Guid RolId { get; set; }
        public Guid idAccion { get; set; }
        public int AvanceId { get; set; }
        public string Usuario { get; set; }
        public String observacion { get; set; }
        public string DireccionIp { get; set; }
    }
}
