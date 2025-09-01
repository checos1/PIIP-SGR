namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AlertasConfigServiciosMock : IAlertasConfigServicios
    {
        public Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            return !string.IsNullOrEmpty(alertasConfigFiltroDto.AlertasConfigDto?.NombreAlerta) ? Task.FromResult(new AlertasConfigDto()) 
                : Task.FromResult<AlertasConfigDto>(null);
        }

        public Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            return alertasConfigFiltroDto.AlertasConfigDto?.Id > 0 ? Task.FromResult(new AlertasConfigDto()) 
                : Task.FromResult<AlertasConfigDto>(null);
        }

        public Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto instanciaAlertasConfigDto)
        {
            return instanciaAlertasConfigDto.ParametrosDto?.IdUsuarioDNP == "jdelgado" ? Task.FromResult(new List<AlertasConfigDto>()) 
                : Task.FromResult<List<AlertasConfigDto>>(null);
        }
    }
}
