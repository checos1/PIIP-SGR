using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.AvalUso
{
    public class DatosAvalUsoDto
    {
        public int Id;
        public int ProyectoId;
        public Guid InstanciaId;
        public bool cumpleAvalUso;
    }
}
