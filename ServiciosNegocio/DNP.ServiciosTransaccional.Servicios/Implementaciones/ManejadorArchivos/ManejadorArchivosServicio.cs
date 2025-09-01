using DNP.ServiciosTransaccional.Servicios.Dto;
using DNP.ServiciosTransaccional.Servicios.Interfaces;
using DNP.ServiciosTransaccional.Servicios.Interfaces.ManejadorArchivos;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.ManejadorArchivos
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Collections.ObjectModel;

    public class ManejadorArchivosServicio : ServicioBase<ArchivoViewModelDto>, IManejadorArchivosServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly IAuditoriaServicios _auditoriaServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
        private static readonly UsuariosArchivosLoginDto usrAuthLogin = new UsuariosArchivosLoginDto();
        private static readonly string _coleccionPIIP = ConfigurationManager.AppSettings["collectionArchivos"];
        private static readonly string _userAuthArchivos = ConfigurationManager.AppSettings["userAuthArchivos"];
        private static readonly string _passwordAuthArchivos = ConfigurationManager.AppSettings["passwordAuthArchivos"];

        static ManejadorArchivosServicio()
        {
            usrAuthLogin.Colleccion = _coleccionPIIP;
            usrAuthLogin.User = _userAuthArchivos;
            usrAuthLogin.Password = _passwordAuthArchivos;
        }

        public ManejadorArchivosServicio(IClienteHttpServicios clienteHttpServicios
            , IAuditoriaServicios auditoriaServicios
            )
            : base(auditoriaServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
            _auditoriaServicios = auditoriaServicios;
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

        protected override object GuardadoDefinitivo(ParametrosGuardarDto<ArchivoViewModelDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Clonar(Dictionary<string, object> parametros, string usuarioDNP)
        {

            string coleccion = string.Empty;
            if (parametros.ContainsKey("coleccion"))
            {
                coleccion = parametros["coleccion"].ToString();
            }

            string urlServicio = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
            var uriMetodo = ConfigurationManager.AppSettings["uriClonarDocumentoSoporte"] + coleccion;

            Task<string> tokenBearer = GetAuthorizationApiArchivos(usrAuthLogin, usuarioDNP);

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.PostAsync,
                    urlServicio,
                    uriMetodo,
                    string.Empty,
                    parametros,
                    usuarioDNP,
                    useBearerToken: true,
                    tokenBearerJWT: tokenBearer.Result
            );

            return true;
        }


    }
}
