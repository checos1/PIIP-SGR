namespace DNP.Backbone.Servicios.Interfaces.Inbox
{
    using System.Threading.Tasks;
    using Comunes.Dto;
    using Dominio.Dto.Inbox;

    public interface IInboxServicios
    {
        Task<InboxDto> ObtenerInfoPDF(InstanciaInboxDto datosConsulta, string token);
        Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token);
        Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto);
    }
}
