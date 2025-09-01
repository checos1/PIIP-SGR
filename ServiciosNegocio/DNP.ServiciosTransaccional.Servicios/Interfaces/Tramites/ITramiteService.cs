

using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites
{
    public interface ITramiteService
    {
        Task<DetalleTramiteDto> ObtenerDetalleTramite(string numeroTramite);
    }
}
