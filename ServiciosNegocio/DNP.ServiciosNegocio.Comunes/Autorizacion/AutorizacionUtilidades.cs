using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace DNP.ServiciosNegocio.Comunes.Autorizacion
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Enum;
    using Excepciones;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [ExcludeFromCodeCoverage]
    public class AutorizacionUtilidades : IAutorizacionUtilidades
    {
        public Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ValidarParametros(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);

            var respuestaAutorizacion = ObtenerPermisosUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);

            if (!respuestaAutorizacion.IsSuccessStatusCode) return Task.FromResult(respuestaAutorizacion);

            ConstruirRespuestaHttp(respuestaAutorizacion);

            var respuestaHttp = new HttpResponseMessage
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    ReasonPhrase = ServiciosNegocioRecursos.UsuarioAutorizado
                                };

            return Task.FromResult(respuestaHttp);
        }

        private static void ConstruirRespuestaHttp(HttpResponseMessage respuestaAutorizacion)
        {
            var mensaje = string.Empty;
            var valido = false;

            var contenido = respuestaAutorizacion.Content.ReadAsStringAsync();
            var resultado = JToken.Parse(contenido.Result);
            var tienePermiso = Convert.ToBoolean(resultado.SelectToken("Permiso").ToString());
            var estados = JsonConvert.DeserializeObject<List<int>>(resultado.SelectToken("Estados").ToString());

            if (estados.Contains((int)EstadoAutorizacion.UsuarioNoExiste))
            {
                mensaje = ServiciosNegocioRecursos.CredencialesInvalidas;
            }
            else if (estados.Contains((int)EstadoAutorizacion.AplicacionNoExiste))
            {
                mensaje = ServiciosNegocioRecursos.AplicacionNoExiste;
            }
            else if (estados.Contains((int)EstadoAutorizacion.OpcionNoExiste))
            {
                mensaje = ServiciosNegocioRecursos.ServicioNoExiste;
            }
            else if (estados.Contains((int)EstadoAutorizacion.UsuarioSinPermisosParaLaAplicacionUOpcion))
            {
                mensaje = ServiciosNegocioRecursos.UsuarioNoTienePermisos;
            }
            else if (estados.Contains((int)EstadoAutorizacion.AutenticacionNoValidaDeLaAplicacionCliente))
            {
                mensaje = ServiciosNegocioRecursos.AplicacionNoExiste;
            }
            else if (estados.Contains((int)EstadoAutorizacion.ErrorIndefinido))
            {
                mensaje = ServiciosNegocioRecursos.ErrorIndefinido;
            }
            else if (!tienePermiso)
            {
                mensaje = ServiciosNegocioRecursos.UsuarioNoTienePermisos;
            }
            else
            {
                valido = true;
            }

            if (!valido)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.Unauthorized, mensaje);

        }

        private void ValidarParametros(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            var mensaje = string.Empty;
            var valido = false;

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "nombreUsuario");
            }
            else if (string.IsNullOrEmpty(hashUsuario))
            {
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "hashUsuario");
            }
            else if (string.IsNullOrEmpty(idAplicacion))
            {
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "idAplicacion");
            }
            else if (string.IsNullOrEmpty(nombreServicio))
            {
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "nombreServicio");
            }
            else
                valido = true;

            if (!valido)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.Unauthorized, mensaje);
        }

        private static HttpResponseMessage ObtenerPermisosUsuario(string nombreUsuario, string hashUsuario, string idAplicacion,
                                                                  string nombreServicio)
        {
            HttpResponseMessage respuestaAutorizacion;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAutorizacion"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var autorizacion = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{nombreUsuario}:{hashUsuario}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", autorizacion);

                var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPermiso"];
                var parametros = $"?idAplicacion={idAplicacion}&idOpcion={nombreServicio}";

                respuestaAutorizacion = client.GetAsync(uriMetodo + parametros).Result;
            }

            return respuestaAutorizacion;
        }
    }
}
