namespace DNP.Backbone.Servicios.Implementaciones.SGP.Tramite
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Servicios.Implementaciones.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.SGP.Tramite;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase responsable de la gestión de servicio del trámites SGP
    /// </summary>
    public class SGPTramiteProyectoServicios: ISGPTramiteProyectoServicios
    {
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>                
        public SGPTramiteProyectoServicios(IServiciosNegocioServicios serviciosNegocioServicios)
        {
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        public async Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnp, string TokenAutorizacion)
        {
            var proyectosTramites = new List<ProyectosEnTramiteDto>();
            proyectosTramites = _serviciosNegocioServicios.ObtenerProyectosTramiteNegocioSGP(TramiteId, usuarioDnp, TokenAutorizacion).Result;
            return await Task.Run(() => proyectosTramites);
        }

        public async Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarProyectosTramiteNegocioSGP(parametros, usuarioDnp);
        }

        public async Task<string> ValidacionProyectosTramiteNegocio(int TramiteId, string usuarioDnp, string TokenAutorizacion)
        {
            return await _serviciosNegocioServicios.ValidacionProyectosTramiteNegocio(TramiteId, usuarioDnp, TokenAutorizacion);
        }
    }
}
