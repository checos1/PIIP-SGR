namespace DNP.Backbone.Servicios.Implementaciones.TramiteIncorporacion
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using Dominio.Dto.Proyecto;
    using Interfaces.Autorizacion;
    using Interfaces.ServiciosNegocio;
    using Interfaces.Tramites;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using DNP.Backbone.Comunes.Utilidades;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Interfaces.Auditoria;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using System.Web.Http;

    public class TramiteIncorporacionServicio : ITramiteIncorporacionServicios
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;
        public TramiteIncorporacionServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }
        
        public async Task<string> ObtenerDatosIncorporacion(int tramiteId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["UriObtenerDatosIncorporacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?tramiteId={tramiteId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objConvenioDonanteDto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<string> GuardarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["UriGuardarDatosIncorporacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objConvenioDonanteDto, usuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objConvenioDonanteDto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<string> EiliminarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["UriEiliminarDatosIncorporacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objConvenioDonanteDto, usuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

    }
}
