using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Transversales
{
    public interface IParametrosPersistencia
    {
        string ConsultarParametro(string llave);
    }
}
