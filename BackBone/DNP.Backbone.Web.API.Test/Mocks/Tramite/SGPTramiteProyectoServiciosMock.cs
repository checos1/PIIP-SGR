namespace DNP.Backbone.Web.API.Test.Mocks.Tramite
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.SGP.Tramite;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SGPTramiteProyectoServiciosMock: ISGPTramiteProyectoServicios
    {
        private readonly ISGPTramiteProyectoServicios _tramiteProyectoSGPServicios;

        Task<List<ProyectosEnTramiteDto>> ISGPTramiteProyectoServicios.ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        Task<RespuestaGeneralDto> ISGPTramiteProyectoServicios.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        Task<string> ISGPTramiteProyectoServicios.ValidacionProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }
    }
}
