namespace DNP.Backbone.Web.API.Test.Mocks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DNP.Backbone.Comunes.Dto;
    using Dominio.Dto;
    using Servicios.Interfaces;

    public class BackboneServiciosMock : IBackboneServicios
    {
        public Task<List<SectorDto>> ConsultarProyectosPorResponsable(string usuarioResponsable)
        {
            if (usuarioResponsable == "jdelgado")
                return Task.FromResult(new List<SectorDto>()
                                       {
                                           new SectorDto()
                                           {
                                               EntidadList =
                                                   new List<EntidadDto>(),
                                               id = 5,
                                               sector = "Sector x"
                                           }
                                       });

            return Task.FromResult(new List<SectorDto>());

        }
        public Task<List<SectorDto>> ConsultarTramitesPorResponsable(string usuarioResponsable)
        {
            if (usuarioResponsable == "jdelgado")
                return Task.FromResult(new List<SectorDto>()
                                       {
                                           new SectorDto()
                                           {
                                               EntidadList =
                                                   new List<EntidadDto>(),
                                               id = 5,
                                               sector = "Sector x"
                                           }
                                       });

            return Task.FromResult(new List<SectorDto>());
        }
        public Task<List<NotificacionesDto>> ConsultarNotificacionPorResponsable(string usuarioResponsable)
        {
            if (usuarioResponsable == "jdelgado")
                return Task.FromResult(new List<NotificacionesDto>()
                                       {
                                           new NotificacionesDto()
                                       });

            return Task.FromResult(new List<NotificacionesDto>());

        }
      
        public bool ActualizarPrioridadTramite(int idsolicitud, string tipoSolicitud, string usuario)
        {
            return idsolicitud == 1 && tipoSolicitud == "tipo1" && usuario == "jdelgado";
        }

        public Task<string> NotificarUsuarios(List<ParametrosCrearNotificacionFlujoDto> parametros, string usuarioDNP)
        {
            return Task.FromResult("OK");
        }
    }
}
