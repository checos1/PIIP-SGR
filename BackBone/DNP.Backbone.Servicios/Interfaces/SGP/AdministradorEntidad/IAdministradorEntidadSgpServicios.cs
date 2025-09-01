using DNP.Backbone.Comunes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad
{
    public interface IAdministradorEntidadSgpServicios
    {
        Task<string> ObtenerSectores(string usuarioDnp);
        Task<string> ObtenerFlowCatalog(string usuarioDnp);
        Task<List<ConfiguracionUnidadMatrizDTO>> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion);
        Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion, string idUsuarioDNP);
    }
}
