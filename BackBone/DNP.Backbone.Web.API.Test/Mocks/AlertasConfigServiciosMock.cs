namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AlertasConfigServiciosMock : IAlertasConfigServicios
    {
        public Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            return Task.FromResult(new AlertasConfigDto());
        }

        public Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            return Task.FromResult(new AlertasConfigDto());
        }

        public Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto instanciaAlertasConfigDto)
        {
            return Task.FromResult(new List<AlertasConfigDto> { new AlertasConfigDto() });
        }
    }
}
