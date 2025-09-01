namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AlertasGeneradasServiciosMock : IAlertasGeneradasServicios
    {
        public Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto filtro)
        {
            return Task.FromResult((ICollection<AlertasGeneradasDto>)new List<AlertasGeneradasDto> { new AlertasGeneradasDto() });
        }
    }
}
