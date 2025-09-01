using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Focalizacion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Focalizacion
{
    public class IndicadoresPoliticaServicio : IIndicadoresPolitica
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public IndicadoresPoliticaServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        #region Obtener Indicadores Politica

        /// <summary>
        /// Obtener Indicadores Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDnp"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ObtenerIndicadoresPolitica(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriIndicadorPoliticaConsultar"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?Bpin={bpin}", null, usuarioDnp, useJWTAuth: false);

            return response;
        }

        #endregion
    }
}
