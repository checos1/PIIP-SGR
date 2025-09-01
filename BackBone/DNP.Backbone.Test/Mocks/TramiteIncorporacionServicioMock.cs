using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using System.Threading.Tasks;

namespace DNP.Backbone.Test.Mocks
{
    public class TramiteIncorporacionServicioMock : ITramiteIncorporacionServicios
    {
        public Task<string> EiliminarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            return Task.FromResult<string>(string.Empty);
        }

        public Task<string> GuardarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            return Task.FromResult<string>(string.Empty);
        }

        public Task<string> ObtenerDatosIncorporacion(int tramiteId, string usuario)
        {
            return Task.FromResult<string>(string.Empty);
        }
    }
}