namespace DNP.Backbone.Servicios.Interfaces.SGP.Tramite
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISGPTramiteProyectoServicios
    {
        Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp);
        Task<string> ValidacionProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion);
    }
}
