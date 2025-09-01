using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.Transversales;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.ManejadorArchivos;
using DNP.Backbone.Servicios.Interfaces.Transversal;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Transversal
{
    /// <summary>
    /// Clase responsable de la gestión de documento soporte
    /// </summary>
    public class DocumentoSoporteController : Base.BackboneBase
    {
        private readonly IDocumentoSoporteServicios _documentoSoporteServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="documentoSoporteServicios">Instancia de servicios de documento soporte</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>       
        public DocumentoSoporteController(IDocumentoSoporteServicios documentoSoporteServicios,
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _documentoSoporteServicios = documentoSoporteServicios;
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
        [Route("api/Transversal/DocumentoSoporte/ObtenerListaTipoDocumentosSoportePorRolTrv")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaTipoDocumentosSoportePorRolTrv(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _documentoSoporteServicios.ObtenerListaTipoDocumentosSoportePorRolTrv(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene lista de archivos PIIP
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="filtros"></param>  
        /// <returns>IList<ArchivoInfoDto></returns> 
        [Route("api/Transversal/ObtenerListadoArchivosPIIP/{coleccion}")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoArchivosPIIP(string coleccion, [FromBody] Dictionary<string, object> parametros)
        {
            try
            {
                var result = await Task.Run(() => _documentoSoporteServicios.ObtenerListadoArchivosPIIP(coleccion, parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene lista de archivos MGA
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="filtros"></param>  
        /// <returns>IList<ArchivoInfoDto></returns> 
        [Route("api/Transversal/ObtenerListadoArchivosMGA/{coleccion}")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoArchivosMGA(string coleccion, [FromBody] Dictionary<string, object> parametros)
        {
            try
            {
                var result = await Task.Run(() => _documentoSoporteServicios.ObtenerListadoArchivosMGA(coleccion, parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene lista de archivos SUIFP
        /// </summary>     
        /// <param name="parametros"></param>   
        /// <returns>SoportesDto</returns> 
        [Route("api/Transversal/ObtenerListadoArchivosSUIFP")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoArchivosSUIFP(FiltroDocumentosDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _documentoSoporteServicios.ObtenerListadoArchivosSUIFP(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}