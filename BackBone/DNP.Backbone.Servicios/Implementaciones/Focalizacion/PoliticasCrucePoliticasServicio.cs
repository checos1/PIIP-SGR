using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Focalizacion;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Focalizacion
{
    public class PoliticasCrucePoliticasServicio : IPoliticasTransversalesCrucePoliticasServicios
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public PoliticasCrucePoliticasServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<string> ObtenerPoliticasTransversalesCrucePoliticas(string Bpin, int IdFuente, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarFocalizacionPoliticaCrucePolitica"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?Bpin={Bpin}&idFuente={IdFuente}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<RespuestaGeneralDto> ActualizarPoliticasTransversalesCrucePoliticas(PoliticasTCrucePoliticasDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarFocalizacionPoliticaCrucePolitica"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }
    }
}
