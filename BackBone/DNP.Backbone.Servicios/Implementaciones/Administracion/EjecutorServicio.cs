namespace DNP.Backbone.Servicios.Implementaciones.Administracion
{
    using Newtonsoft.Json;
    using System.Configuration;
    using System.Threading.Tasks;

    using System;
    using DNP.Backbone.Servicios.Interfaces.Administracion;
    using DNP.Backbone.Dominio.Dto.Administracion;
    using DNP.Backbone.Comunes.Enums;
    using Interfaces;
    using DNP.Backbone.Comunes.Extensiones;

    public class EjecutorServicio : IEjecutorServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];


        public EjecutorServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<EjecutorDto> ConsultarEjecutor(string nit, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarEjecutor"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, "?nit=" + string.Join("&nit=", nit), null, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<EjecutorDto>(respuesta);
        }

        public async Task<bool> GuardarEjecutor(EjecutorDto Obj, string UsuarioDNP)
        {
            if (Obj.Id == 0) { Obj.CreadoPor = UsuarioDNP;} else { Obj.ModificadoPor = UsuarioDNP; }
                
            Obj.ModificadoPor = UsuarioDNP;
            Obj.FechaModificacion = DateTime.Now;

            var uri = ConfigurationManager.AppSettings["uriGuardarEjecutor"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, Obj, UsuarioDNP, useJWTAuth: false);
            respuesta.TryDeserialize<bool?>(out var result);

            return (bool)result;
        }
    }
}
