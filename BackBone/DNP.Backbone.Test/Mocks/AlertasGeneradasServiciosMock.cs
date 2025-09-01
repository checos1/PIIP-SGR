namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AlertasGeneradasServiciosMock : IAlertasGeneradasServicios
    {
        public Task<ICollection<AlertasGeneradasDto>> ObtenerAlertasGeneradas(AlertasGeneradasFiltroDto filtro)
        {
            ICollection<AlertasGeneradasDto> alertasGeneradas = new List<AlertasGeneradasDto>();
            return Task.FromResult(alertasGeneradas);
        }
    }
}
