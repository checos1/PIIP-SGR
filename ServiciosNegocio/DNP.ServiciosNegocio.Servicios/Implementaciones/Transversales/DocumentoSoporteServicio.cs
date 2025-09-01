namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    using System;
    using DNP.ServiciosNegocio.Comunes.Enum;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces.Transversales;
    using Newtonsoft.Json;
    using System.Linq;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces;

    /// <summary>
    /// Clase responsable de la gestión de documento soporte
    /// </summary>
    public class DocumentoSoporteServicio : IDocumentoSoporteServicio
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly IDocumentoSoportePersistencia _documentoSoportePersistencia;

        //PIIP
        private static readonly UsuariosArchivosLoginDto usrAuthLogin = new UsuariosArchivosLoginDto();
        private static readonly string _coleccionPIIP = ConfigurationManager.AppSettings["collectionArchivos"];
        private static readonly string _userAuthArchivos = ConfigurationManager.AppSettings["userAuthArchivos"];
        private static readonly string _passwordAuthArchivos = ConfigurationManager.AppSettings["passwordAuthArchivos"];

        //MGA
        private static readonly string _coleccionMGA = ConfigurationManager.AppSettings["collectionArchivosMGA"];
        private static readonly string _userAuthArchivosMGA = ConfigurationManager.AppSettings["userAuthArchivosMGA"];
        private static readonly string _passwordAuthArchivosMGA = ConfigurationManager.AppSettings["passwordAuthArchivosMGA"];
        private static readonly string _ambienteMGA = ConfigurationManager.AppSettings["ambienteMGA"];

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="documentoSoportePersistencia">Instancia de persistencia de documento soporte</param>        
        public DocumentoSoporteServicio(IDocumentoSoportePersistencia documentoSoportePersistencia, IClienteHttpServicios clienteHttpServicios)
        {
            _documentoSoportePersistencia = documentoSoportePersistencia;
            _clienteHttpServicios = clienteHttpServicios;
        }

        static DocumentoSoporteServicio()
        {
            usrAuthLogin.Colleccion = _coleccionPIIP;
            usrAuthLogin.User = _userAuthArchivos;
            usrAuthLogin.Password = _passwordAuthArchivos;
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
        public string ObtenerListaTipoDocumentosSoportePorRolTrv(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            return _documentoSoportePersistencia.ObtenerListaTipoDocumentosSoportePorRolTrv(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId);
        }

        /// <summary>
        /// GetAuthorizationApiArchivos
        /// </summary>      
        /// <param name="parametros"></param>   
        /// <param name="usuarioDNP"></param>
        /// <returns>Task<string></returns> 
        public async Task<string> GetAuthorizationApiArchivos(UsuariosArchivosLoginDto parametros, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
            var uri = ConfigurationManager.AppSettings["uriAuthorizationManejadorArchivos"];
            return await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: false);
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
            var urlBase = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
            var uri = ConfigurationManager.AppSettings["uriArchivoInfoManejadorArchivos"] + coleccion;
            var usrAuthLoginMGA = new UsuariosArchivosLoginDto()
            {
                Colleccion = _coleccionMGA,
                User = _userAuthArchivosMGA,
                Password = _passwordAuthArchivosMGA
            };
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLoginMGA, usuarioDNP);
            if (parametros.Count(x => x.Key == "collectionMetadata") == 0)
                parametros.Add("collectionMetadata", _coleccionMGA);
            if (parametros.Count(x => x.Key == "ambiente") == 0)
                parametros.Add("ambiente", _ambienteMGA);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<IList<ArchivoInfoDto>>(respuesta);
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
            var urlBase = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
            var uri = ConfigurationManager.AppSettings["uriArchivoInfoManejadorArchivos"] + coleccion;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            parametros.Add("collectionMetadata", _coleccionPIIP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<IList<ArchivoInfoDto>>(respuesta);
        }
    }
}
