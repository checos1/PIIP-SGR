using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesDistribucion;
using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesDistribucion;
using System;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.TramitesDistribucion
{
    public class TramitesDistribucionServicio : ITramitesDistribucionServicio
    {
        private readonly ITramitesDistribucionPersistencia _tramitesDistribucionPersistencia;

        public TramitesDistribucionServicio(ITramitesDistribucionPersistencia tramitesDistribucionPersistencia)
        {
            _tramitesDistribucionPersistencia = tramitesDistribucionPersistencia;
        }

        public string ObtenerTramitesDistribucionAnteriores(Nullable<Guid> instanciaId)
        {
            return _tramitesDistribucionPersistencia.ObtenerTramitesDistribucionAnteriores(instanciaId);
        }
    }
}
