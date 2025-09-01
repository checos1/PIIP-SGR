namespace DNP.Backbone.Web.API.Test.Mock
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dominio.Dto;
    using Servicios.Interfaces;

    public class BackboneServiciosMock : IBackboneServicios
    {
        public bool ActualizarEstadoProyecto(string bpin, string estado, string usuario)
        {
            return true;
        }

        public bool ActualizarPrioridadTramite(int idsolicitud, string tipoSolicitud, string usuario)
        {
            return true;
        }

        public void ActualizarEstadoNotificaciones(int idNotificacion, string estado)
        {

        }

        Task<List<SectorDto>> IBackboneServicios.ConsultarProyectosPorResponsable(string usuarioResponsable)
        {
            throw new System.NotImplementedException();
        }

        Task<List<SectorDto>> IBackboneServicios.ConsultarTramitesPorResponsable(string usuarioResponsable)
        {
            return Task.FromResult(new List<SectorDto>() { new SectorDto() { } });
        }

        Task<List<NotificacionesDto>> IBackboneServicios.ConsultarNotificacionPorResponsable(string usuarioResponsable)
        {
            throw new System.NotImplementedException();
        }
    }
}
