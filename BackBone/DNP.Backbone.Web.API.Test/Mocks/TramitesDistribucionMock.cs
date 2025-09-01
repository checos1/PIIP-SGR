using DNP.Backbone.Servicios.Interfaces.TramitesDistribucion;
using System;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class TramitesDistribucionMock : ITramitesDistribucionServicios
    {
        public Task<string> ObtenerTramitesDistribucionAnteriores(Guid? instanciaId, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
