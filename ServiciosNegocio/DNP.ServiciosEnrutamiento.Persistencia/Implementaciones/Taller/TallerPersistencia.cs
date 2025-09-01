using DNP.ServiciosEnrutamiento.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosEnrutamiento.Persistencia.Implementaciones
{
    public class TallerPersistencia: Persistencia, ITallerPersistencia
    {
        public TallerPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bpin"></param>
        /// <returns></returns>
        public bool ObtenerEstadoProyecto(string bpin)
        {
            try
            {
                var proyecto = Contexto.Proyecto.Where(proy => proy.BPIN == bpin).FirstOrDefault();
                if (proyecto != null)
                    return proyecto.PorCerrar;
                else
                    throw new ServiciosNegocioException("Proyecto No Existe");
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
