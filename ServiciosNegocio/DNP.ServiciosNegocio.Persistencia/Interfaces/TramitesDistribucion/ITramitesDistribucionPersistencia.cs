using System;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesDistribucion
{
    public interface ITramitesDistribucionPersistencia
    {
        string ObtenerTramitesDistribucionAnteriores(Nullable<Guid> instanciaId);
    }
}
