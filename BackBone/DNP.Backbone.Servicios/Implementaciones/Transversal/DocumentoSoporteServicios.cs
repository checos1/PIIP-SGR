using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Transversal;
using System;
using System.Threading.Tasks;
using DNP.Backbone.Comunes.Enums;
using Newtonsoft.Json;
using System.Configuration;
using DNP.Backbone.Dominio.Dto.ManejadorArchivos;
using System.Collections.Generic;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Dominio.Dto.Transversales;

namespace DNP.Backbone.Servicios.Implementaciones.Transversal
{
    public class DocumentoSoporteServicios : IDocumentoSoporteServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;      
          
        public DocumentoSoporteServicios(IClienteHttpServicios clienteHttpServicios)
        {
            this._clienteHttpServicios = clienteHttpServicios;
        }  

        /// <summary>
        /// Obtiene lista de documento soporte por rol
        /// </summary>     
        /// <param name="tipoTramiteId"></param>   
        /// <param name="roles"></param>   
        /// <param name="tramiteId"></param>   
        /// <param name="nivelId"></param>  
        /// <param name="instanciaId"></param> 
        /// <param name="accionId"></param> 
        /// <returns>string</returns> 
        public async Task<string> ObtenerListaTipoDocumentosSoportePorRolTrv(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerListaTipoDocumentosSoportePorRolTrv"];
            uri = $"{uri}?tipoTramiteId={tipoTramiteId}&roles={roles}&tramiteId={tramiteId}&nivelId={nivelId}&instanciaId={instanciaId}&accionId={accionId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (response != "")
                response = JsonConvert.DeserializeObject<string>(response);
            return response;
        }   
 
        /// <summary>
        /// Obtiene lista de archivos MGA
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        public async Task<IList<ArchivoInfoDto>> ObtenerListadoArchivosMGA(string coleccion, Dictionary<string, object> parametros, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerListadoArchivosMGA"] + coleccion;
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<IList<ArchivoInfoDto>>(response);
        }

        /// <summary>
        /// Obtiene lista de archivos PIIP
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        public async Task<IList<ArchivoInfoDto>> ObtenerListadoArchivosPIIP(string coleccion, Dictionary<string, object> parametros, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerListadoArchivosPIIP"] + coleccion;
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<IList<ArchivoInfoDto>>(response);
        }

        /// <summary>
        /// Obtiene lista de archivos SUIFP
        /// </summary>   
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        public async Task<SoportesDto> ObtenerListadoArchivosSUIFP(FiltroDocumentosDto parametros, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerSoportesProyecto"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametros, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<SoportesDto>(response);
        }
    }
}
