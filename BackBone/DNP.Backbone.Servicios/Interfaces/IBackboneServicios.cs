
namespace DNP.Backbone.Servicios.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DNP.Backbone.Comunes.Dto;
    using Dominio.Dto;

    public interface IBackboneServicios
    {
        Task<List<NotificacionesDto>> ConsultarNotificacionPorResponsable(string usuarioResponsable);
        Task<string> NotificarUsuarios(List<ParametrosCrearNotificacionFlujoDto> parametros, string usuarioDNP);
    }
}
