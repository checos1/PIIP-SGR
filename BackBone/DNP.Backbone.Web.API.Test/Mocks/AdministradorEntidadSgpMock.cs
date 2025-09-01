using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class AdministradorEntidadSgpMock: IAdministradorEntidadSgpServicios
    {
        public Task<string> ObtenerSectores(string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerFlowCatalog(string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<List<ConfiguracionUnidadMatrizDTO>> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion, string idUsuarioDNP)
        {
            throw new NotImplementedException();
        }
    }
}
