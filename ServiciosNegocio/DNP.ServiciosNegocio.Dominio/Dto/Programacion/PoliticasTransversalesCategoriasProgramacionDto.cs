using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class PoliticasTransversalesCategoriasProgramacionDto
    {
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public int PoliticaId { get; set; }
        public List<DatosDimension> DatosDimension { get; set; }
    }
}
