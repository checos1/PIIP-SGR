namespace DNP.Backbone.Servicios.Implementaciones
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Dominio.Dto;
    using System.Configuration;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using System;

    public class BackboneServicios : IBackboneServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiNotificacion"];

        public BackboneServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public BackboneServicios()
        {
        }

        public Task<List<NotificacionesDto>> ConsultarNotificacionPorResponsable(string usuarioResponsable)
        {
            return Task.FromResult(ConsultarNotificacionesPorResponsable());
        }

        private List<NotificacionesDto> ConsultarNotificacionesPorResponsable()
        {
            return new List<NotificacionesDto>();
        }

        public async Task<string> NotificarUsuarios(List<ParametrosCrearNotificacionFlujoDto> parametros, string usuarioDNP)
        {
            DateTime fechactual = DateTime.Now;
            DateTime fechafin = fechactual.AddDays(3);
            parametros.ForEach(x => x.FechaFin = fechafin);
            var uriMetodo = ConfigurationManager.AppSettings["uriNotificarUsuarios"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, parametros, usuarioDNP, useJWTAuth: false);
            return "OK";
        }

    }
}
