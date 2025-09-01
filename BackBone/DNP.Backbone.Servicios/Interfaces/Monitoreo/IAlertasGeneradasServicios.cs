using DNP.Backbone.Comunes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Monitoreo
{
    public interface IAlertasGeneradasServicios
    {
        Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto filtro);
    }
}
