namespace DNP.Backbone.Servicios.Interfaces.Monitoreo
{
    using Comunes.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAlertasConfigServicios
    {
        Task<List<AlertasConfigDto>> ObtenerAlertasConfig(AlertasConfigFiltroDto instanciaAlertasConfigDto);
        Task<AlertasConfigDto> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto);
        Task<AlertasConfigDto> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto);
    }
}
