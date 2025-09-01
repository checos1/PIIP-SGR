using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesDistribucion;
using System;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesDistribucion
{
    public class TramitesDistribucionPersistencia: Persistencia, ITramitesDistribucionPersistencia
    {
        #region Constructor

        public TramitesDistribucionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        public string ObtenerTramitesDistribucionAnteriores(Nullable<Guid> instanciaId)
        {
            var listaTramitesDistribucion = Contexto.uspGetResumenTramiteDistribuciones_JSON(instanciaId).FirstOrDefault();
            return listaTramitesDistribucion;
        }
    }
}
