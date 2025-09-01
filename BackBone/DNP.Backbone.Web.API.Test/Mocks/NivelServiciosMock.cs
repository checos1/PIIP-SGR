using DNP.Backbone.Dominio.Dto.Nivel;
using DNP.Backbone.Servicios.Interfaces.Nivel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class NivelServiciosMock : INivelServicios
    {
        public Task<List<NivelDto>> ObtenerPorIdPadreIdNivelTipo(Guid? idPadre, string claveNivelTipo, string idUsuarioDnp)
        {
            return Task.FromResult(new List<NivelDto>() { new NivelDto() });
        }
    }
}
