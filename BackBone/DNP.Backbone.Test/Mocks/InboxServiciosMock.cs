namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Properties;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.Inbox;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InboxServiciosMock : IInboxServicios
    {
        public Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }

        public Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new InboxDto()
            {
                Mensaje = Resources.UsuarioNoTieneTareasPendientes,
                GruposEntidades = new List<GrupoEntidadDto>() { new GrupoEntidadDto() }
            });
        }

        public Task<InboxDto> ObtenerInfoPDF(InstanciaInboxDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }
    }
}
