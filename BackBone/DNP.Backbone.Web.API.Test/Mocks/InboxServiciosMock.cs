namespace DNP.Backbone.Web.API.Test.Mocks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Properties;
    using Dominio.Dto.Inbox;
    using Servicios.Interfaces.Inbox;

    public class InboxServiciosMock : IInboxServicios
    {
        public Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token) 
        {
            if (datosConsulta == null || string.IsNullOrEmpty(datosConsulta.Aplicacion) || string.IsNullOrEmpty(datosConsulta.IdUsuario) 
                || datosConsulta.ListaIdsRoles == null || datosConsulta.ListaIdsRoles.Count == 0)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
            return Task.FromResult(new InboxDto() { Mensaje = "Prueba Ok" }); 
        }

        public Task<InboxDto> ObtenerInbox(ParametrosInboxDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            return Task.FromResult(new InboxDto()
            {
                Mensaje = "Prueba Ok",
                GruposEntidades = new List<GrupoEntidadDto>() { new GrupoEntidadDto() }
            });
        }

        public Task<InboxDto> ObtenerInfoPDF(InstanciaInboxDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }
    }
}
