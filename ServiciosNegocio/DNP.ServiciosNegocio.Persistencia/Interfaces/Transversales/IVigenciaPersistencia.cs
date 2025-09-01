using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales
{
    public interface IVigenciaPersistencia
    {
        List<int?> ObtenerVigencias();
    }
}
