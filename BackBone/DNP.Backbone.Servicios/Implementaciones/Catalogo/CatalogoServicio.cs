using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Catalogo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Catalogo
{
    public class CatalogoServicio : ICatalogoServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        public async Task<List<CatalogoDto>> ObtenerCatalogo(string catalogo, string tokenAutorizacion, string uriMetodo)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tokenAutorizacion);

                var response = client.PostAsJsonAsync(endPoint + uriMetodo, catalogo).Result;

                return JsonConvert.DeserializeObject<List<CatalogoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, null, useJWTAuth: false));
            }
        }
    }
}
