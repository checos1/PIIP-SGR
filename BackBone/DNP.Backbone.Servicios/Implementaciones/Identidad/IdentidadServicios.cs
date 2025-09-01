using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Identidad;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Identidad
{
    public class IdentidadServicios : IIdentidadServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiIdentidad"];


        public IdentidadServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<bool> InvitarUsuario(InvitarUsuarioDto dto, string usuarioDnp, bool esUsuarioBackbone)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriIdentidadInvitarUsuarioV2"];
            var request = new InvitarUsuarioB2BRequestDto
            {
                EsUsuarioBackbone = esUsuarioBackbone,
                GivenName = dto.Nombre,
                InvitedUserDisplayName = dto.Nombre + " " + dto.Apellido,
                InvitedUserEmailAddress = dto.Correo,
                Surname = dto.Apellido,
                Identificacion = dto.Identificacion,
                TipoIdentificacion = dto.TipoIdentificacion,
                IdUsuarioDNP = dto.IdUsuarioDNP
    };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostAsync, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false);
            respuesta.TryDeserialize<bool?>(out var result);

            if (result == null)
            {
                var errores = respuesta.Deserialize<IDictionary<string, string>>();
                throw new BackboneException(errores["Message"]);
            }

            return (bool)result;
        }

        public async Task<bool> CambiarClaveUsuario(CredencialUsuarioDto request, string usuarioDnp)
        {
            var uriMetodo = "api/Identidad/cambiarClaveUsuarioB2C";
            var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, request, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }


        public async Task<InvitarUsuarioDto> ObtenerUsuarioDominio(string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPorIdUsuarioDnp"];
            var parametros = $"?id={idUsuarioDNP}";
            var tResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDNP, useJWTAuth: false));
            var usuarioDto = new InvitarUsuarioDto();

            if (tResult != null)
                usuarioDto = new InvitarUsuarioDto
                {
                    Nombre = tResult["givenName"].ToString(),
                    Apellido = tResult["surname"].ToString(),
                    Correo = tResult["mail"].ToString(),
                    TipoIdentificacion = tResult["extension_69e1117a3abb4f519a7cfe27ebe44991_TipoIdentificacion"].ToString(),
                    Identificacion = tResult["extension_69e1117a3abb4f519a7cfe27ebe44991_NumeroIdentificacion"].ToString()
                };
            return usuarioDto;
        }

        public async Task<bool> CambiarContrasenaSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCambiarContrasenaSTS"];
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, usuarioSTS, idUsuarioDNP, useJWTAuth: false));
          
            return tResult;
        }

        public async Task<string> EnviarCorreoInvitacionSTS(NotificarInvitacionUsuarioSTSDto notificacion, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEnviarCorreoInvitacionSTS"]; 
            var tResult = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, notificacion, idUsuarioDNP, useJWTAuth: false));

            return tResult;
        }

        public async Task<bool> RegistrarUsuarioAPPSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarUsuarioAPPSTS"]; 
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, usuarioSTS, idUsuarioDNP, useJWTAuth: false));

            return tResult;
        }

        public async Task<bool> RegistrarUsuarioSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarUsuarioSTS"];
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, usuarioSTS, idUsuarioDNP, useJWTAuth: false));

            return tResult;
           
        }

        public async Task<bool> ValidarContrasenaActualSTS(UsuarioSTSDto usuarioSTS, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarContrasenaActualSTS"];
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, usuarioSTS, idUsuarioDNP, useJWTAuth: false));

            return tResult;
        }

		public async Task<bool> apiIdentidadVerificarExistenciaUsuarioSTSAplicacion(string pAplicacion, string pTD, string pNumeroDocumento, string idUsuarioDNP)
		{
            var uriMetodo = ConfigurationManager.AppSettings["VerificarExistenciaUsuarioSTSAplicacion"];
            uriMetodo = uriMetodo + "?pAplicacion=" + pAplicacion + "&pTD=" + pTD + "&pNumeroDocumento=" + pNumeroDocumento;
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, string.Empty, null, idUsuarioDNP, useJWTAuth: false));

            return tResult;
        }

        public async Task<bool> apiObtenerAplicacionesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["VerificarExistenciaUsuarioSTSAplicacion"];
            uriMetodo = uriMetodo + "?pAplicacion=" + pAplicacion + "&pTD=" + pTD + "&pNumeroDocumento=" + pNumeroDocumento;
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, string.Empty, null, idUsuarioDNP, useJWTAuth: false));

            return tResult;
        }

        public async Task<bool> apiObtenerAplicacionesConfiablesExistenciaUsuarioSTS(string pAplicacion, string pTD, string pNumeroDocumento, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["VerificarExistenciaUsuarioSTSAplicacion"];
            uriMetodo = uriMetodo + "?pAplicacion=" + pAplicacion + "&pTD=" + pTD + "&pNumeroDocumento=" + pNumeroDocumento;
            var tResult = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, string.Empty, null, idUsuarioDNP, useJWTAuth: false));

            return tResult;
        }

        /// <summary>
        /// Consulta de usuario en Azure AD.
        /// </summary>
        /// <param name="id"></param>
        public async Task<UsuarioDNPDto> ObtenerUsuarioPorId(string id, string idUsuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPorId"];
            uriMetodo = uriMetodo + "?id=";
           
            var resultadoUsuario = JsonConvert.DeserializeObject<UsuarioDNPDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, id, null, idUsuarioDNP, useJWTAuth: false));
            return resultadoUsuario;
        }
    }
}
