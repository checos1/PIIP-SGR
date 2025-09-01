namespace DNP.ServiciosNegocio.Web.API.Controllers.Transversales
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes.Autorizacion;
    using Comunes.Excepciones; 
    using Swashbuckle.Swagger.Annotations;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;  
    using System;
    using System.Collections.Generic;
   
    /// <summary>
    /// Clase Api responsable de la gestion de documento soporte
    /// </summary>
    public class DocumentoSoporteController : ApiController
    {
        private readonly IDocumentoSoporteServicio _documentoSoporte;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="documentoSoporte">Instancia de servicios de documento soporte</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de documento soporte</param>       
        public DocumentoSoporteController(IDocumentoSoporteServicio documentoSoporte, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _documentoSoporte = documentoSoporte;
            _autorizacionUtilidades = autorizacionUtilidades;
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
        [Route("api/ObtenerListaTipoDocumentosSoportePorRolTrv")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene lista de documento soporte por rol", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaTipoDocumentosSoportePorRolTrv(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerTipoDocumentoSoporte"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _documentoSoporte.ObtenerListaTipoDocumentosSoportePorRolTrv(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId));

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        /// <summary>
        /// Obtiene lista de archivos PIIP
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="parametros"></param>        
        /// <returns>IList<ArchivoInfoDto></returns> 
        [Route("api/ObtenerListadoArchivosPIIP/{coleccion}")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene lista de archivos PIIP", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoArchivosPIIP(string coleccion, [FromBody] Dictionary<string, object> parametros)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerTipoDocumentoSoporte"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _documentoSoporte.ObtenerListadoArchivosPIIP(coleccion, parametros, RequestContext.Principal.Identity.Name));

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        /// <summary>
        /// Obtiene lista de archivos MGA
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="parametros"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        [Route("api/ObtenerListadoArchivosMGA/{coleccion}")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene lista de archivos MGA", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoArchivosMGA(string coleccion, [FromBody] Dictionary<string, object> parametros)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                   ConfigurationManager.AppSettings["SGR_Transversal_ObtenerTipoDocumentoSoporte"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _documentoSoporte.ObtenerListadoArchivosMGA(coleccion, parametros, RequestContext.Principal.Identity.Name));

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

    }
}