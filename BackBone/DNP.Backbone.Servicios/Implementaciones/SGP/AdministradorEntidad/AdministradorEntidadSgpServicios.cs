using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces;
using System.Configuration;
using System.Threading.Tasks;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad;
using DNP.Backbone.Comunes.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DNP.Backbone.Servicios.Implementaciones.SGP.AdministradorEntidad
{
    public class AdministradorEntidadSgpServicios: IAdministradorEntidadSgpServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IAutorizacionServicios _autorizacionServicios;

        public AdministradorEntidadSgpServicios(IClienteHttpServicios clienteHttpServicios, IAutorizacionServicios autorizacionServicios)
        {
            this._clienteHttpServicios = clienteHttpServicios;
            _autorizacionServicios = autorizacionServicios;
        }

        public async Task<string> ObtenerSectores(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSectoresSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerFlowCatalog(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerFlowCatalogSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// Lista de estado del proyecto
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <returns>Task<List<EstadoDto>></returns>
        public async Task<List<ConfiguracionUnidadMatrizDTO>> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerMatrizEntidadDestinoSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ConfiguracionUnidadMatrizDTO>>(respuesta);
        }


        /// <summary>
        /// Lista de estado del proyecto
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <returns>Task<List<EstadoDto>></returns>
        public async Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion, string idUsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriActualizarMatrizEntidadDestinoSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uri, string.Empty, peticion, idUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }
    }
}
