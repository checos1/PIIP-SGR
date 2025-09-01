using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales
{
    public class VigenciaPersistencia : Persistencia, IVigenciaPersistencia
    {
        public VigenciaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public List<int?> ObtenerVigencias()
        {
            var resultSp = Contexto.uspGetVigencias().ToList();
            return resultSp;
        }
    }
}
