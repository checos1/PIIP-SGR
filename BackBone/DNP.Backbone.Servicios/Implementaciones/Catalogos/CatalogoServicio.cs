namespace DNP.Backbone.Servicios.Implementaciones.Catalogos
{
    using Newtonsoft.Json;
    using System.Configuration;
    using System.Threading.Tasks;

    using System;
    using DNP.Backbone.Dominio.Dto.Catalogos;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Servicios.Interfaces.Catalogos;
    using Interfaces;
    using System.Collections.Generic;

    public class CatalogoServicio : ICatalogoServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];


        public CatalogoServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<List<CatalogoDto>> consultarCatalogo(string usuarioDnp, CatalogoEnum catalogo)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriListaCatalogo"] + catalogo.ToString(); 
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<CatalogoDto>>(respuesta);
        }

        public async Task<string> ObtenerTablasBasicas(string jsonCondicion, string Tabla, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTablasBasicas"];
            uriMetodo += "?jsonCondicion=" + jsonCondicion + "&Tabla=" + Tabla;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
