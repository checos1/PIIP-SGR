using System;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.TramitesDistribucion
{
    public interface ITramitesDistribucionServicio
    {
        string ObtenerTramitesDistribucionAnteriores(Nullable<Guid> instanciaId);
    }
}
