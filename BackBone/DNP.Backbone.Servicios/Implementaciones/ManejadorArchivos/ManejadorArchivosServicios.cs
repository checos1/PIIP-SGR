using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.ManejadorArchivos;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.ManejadorArchivos;
using DNP.Backbone.Comunes.Utilidades;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Dominio.Dto.Transversales;

namespace DNP.Backbone.Servicios.Implementaciones.ManejadorArchivos
{
    public class ManejadorArchivosServicios : IManejadorArchivosServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
        private static readonly UsuariosArchivosLoginDto usrAuthLogin = new UsuariosArchivosLoginDto();
        private static readonly string _coleccionPIIP = ConfigurationManager.AppSettings["collectionArchivos"];
        private static readonly string _userAuthArchivos = ConfigurationManager.AppSettings["userAuthArchivos"];
        private static readonly string _passwordAuthArchivos = ConfigurationManager.AppSettings["passwordAuthArchivos"];

        static ManejadorArchivosServicios()
        {
            usrAuthLogin.Colleccion = _coleccionPIIP;
            usrAuthLogin.User = _userAuthArchivos;
            usrAuthLogin.Password = _passwordAuthArchivos;
        }

        public ManejadorArchivosServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }        

        public async Task<ArchivoInfoViewModelDto> ObtenerArchivoInfo(string Id, string coleccion, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivoInfoManejadorArchivos"] + coleccion + "/" + _coleccionPIIP + "/" + Id;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<ArchivoInfoViewModelDto>(respuesta);
        }

        public async Task<IList<ArchivoInfoDto>> ObtenerListadoArchivos(string coleccion, Dictionary<string, object> parametros, string usuarioDNP)
        {            
            var uri = ConfigurationManager.AppSettings["uriArchivoInfoManejadorArchivos"] + coleccion;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            parametros.Add("collectionMetadata", _coleccionPIIP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<IList<ArchivoInfoDto>>(respuesta);
        }

        public async Task<List<ArchivoInfoDto>> ValidarArchivoSgr(string coleccion, Dictionary<string, object> parametros, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriValidarArchivosSgr"] + coleccion;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            parametros.Add("collectionMetadata", _coleccionPIIP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<List<ArchivoInfoDto>>(respuesta);
        }

        public async Task<bool> GuardarArchivoRepositorio(ArchivoInfoBsonDto parametros, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivoInfoManejadorArchivos"];
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            Dictionary<string, object> metadatos = parametros.metadatos;
            metadatos.Add("collectionMetadata", _coleccionPIIP);
            parametros.metadatos = metadatos;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        public async Task<bool> CambiarEstadoDataArchivo(string coleccion, string idArchivo, string status, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivosManejadorArchivos"] + coleccion + "/" + _coleccionPIIP + "/" + idArchivo + "/" + status;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Delete, urlBase, uri, string.Empty, null, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        public async Task<byte[]> ObtenerArchivoBytes(string coleccion, string IdArchivoBlob, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivosManejadorArchivos"] + coleccion + "/" + _coleccionPIIP + "/" + IdArchivoBlob;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            return await _clienteHttpServicios.RequestApiByteArray(MetodosServiciosWeb.Get, urlBase + uri, string.Empty, null, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
        }

        public async Task<bool> EliminarArchivo(string coleccion, string idArchivo, string status, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivosManejadorArchivos"] + coleccion + "/" + _coleccionPIIP + "/" + idArchivo + "/" + status;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Delete, urlBase, uri, string.Empty, null, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        public async Task<bool> ActualizarArchivo(string coleccion, string id, ArchivoInfoViewModelDto archivo, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivoInfoManejadorArchivos"] + coleccion + "/" + id;
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Put, urlBase, uri, string.Empty, archivo, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        public async Task<IList<ResponseArchivoViewModelDto>> CargarArchivos(ArchivoViewModelDto archivo, Stream file, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriArchivosManejadorArchivos"];
            var metadatos = JsonConvert.DeserializeObject<Dictionary<string, object>>(archivo.Metadatos);
            metadatos.Add("collectionMetadata", _coleccionPIIP);
            var formData = new
            {
                FormFile = archivo.FormFile,
                nombre = archivo.Nombre,
                coleccion = archivo.Coleccion,
                status = archivo.Status,
                Metadatos = JsonConvert.SerializeObject(metadatos), //archivo.Metadatos,
                Bytes = ConvertUtilidades.StreamToByteArray(file),
                usuario = usuarioDNP
            };

            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            var file_content = new ByteArrayContent(formData.Bytes, 0, formData.Bytes.Length);
            file_content.Headers.ContentType = new MediaTypeHeaderValue("application/" + Path.GetExtension(formData.nombre));

            multiContent.Add(file_content, "FormFile", formData.nombre);
            multiContent.Add(new StringContent(formData.nombre), "nombre");
            multiContent.Add(new StringContent(formData.usuario), "usuario");
            multiContent.Add(new StringContent(formData.coleccion), "coleccion");
            multiContent.Add(new StringContent(formData.status), "status");
            multiContent.Add(new StringContent(formData.Metadatos), "Metadatos");

            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            HttpResponseMessage response = await _clienteHttpServicios.PostRequestApiMultiContent(urlBase + uri, multiContent, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);

            return JsonConvert.DeserializeObject<IList<ResponseArchivoViewModelDto>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<string> GetAuthorizationApiArchivos(UsuariosArchivosLoginDto parametros, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriAuthorizationManejadorArchivos"];
            return await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, usuarioDNP, useBearerToken: false);
        }

        public async Task<ArchivoDto> ObtenerArchivoDocumentos(List<DocumentosDto> datos, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriDescargaMasivaDocumentos"];
            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, datos, usuarioDNP, useBearerToken: true, tokenBearerJWT: tokenBearer.Result);
            return JsonConvert.DeserializeObject<ArchivoDto>(respuesta);
        }
    }
}
