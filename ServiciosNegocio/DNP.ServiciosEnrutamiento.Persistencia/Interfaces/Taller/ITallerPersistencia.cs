using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Persistencia.Interfaces
{
    public interface ITallerPersistencia
    {
        bool ObtenerEstadoProyecto(string bpin);
    }
}
