using DNP.Backbone.Dominio.Dto.ManejadorArchivos;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Transversal
{
    public interface IDocumentoSoporteServicios
    {
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
        Task<string> ObtenerListaTipoDocumentosSoportePorRolTrv(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId, string usuarioDNP);

        /// <summary>
        /// Obtiene lista de archivos PIIP
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        Task<IList<ArchivoInfoDto>> ObtenerListadoArchivosPIIP(string coleccion, Dictionary<string, object> parametros, string usuarioDNP);

        /// <summary>
        /// Obtiene lista de archivos MGA
        /// </summary>     
        /// <param name="coleccion"></param>   
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        Task<IList<ArchivoInfoDto>> ObtenerListadoArchivosMGA(string coleccion, Dictionary<string, object> parametros, string usuarioDNP);

        /// <summary>
        /// Obtiene lista de archivos SUIFP
        /// </summary>        
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>IList<ArchivoInfoDto></returns> 
        Task<SoportesDto> ObtenerListadoArchivosSUIFP(FiltroDocumentosDto parametros, string usuarioDNP);
    }
}
