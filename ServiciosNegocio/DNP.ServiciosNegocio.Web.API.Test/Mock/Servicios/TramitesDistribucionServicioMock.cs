using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesDistribucion;
using System;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class TramitesDistribucionServicioMock : ITramitesDistribucionServicio
    {
        public string ObtenerTramitesDistribucionAnteriores(Guid? instanciaId)
        {
            return string.Empty;
        }
    }
}
