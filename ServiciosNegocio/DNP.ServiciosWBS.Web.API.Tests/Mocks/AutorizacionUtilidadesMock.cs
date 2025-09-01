namespace DNP.ServiciosWBS.Web.API.Tests.Mocks
{
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Excepciones;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class AutorizacionUtilidadesMock : IAutorizacionUtilidades
    {
        [ExcludeFromCodeCoverage]
        public Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario,
                                                        string idAplicacion, string nombreServicio)
        {
            ValidarParametros(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);

            var respuestaAutorizacion = ObtenerPermisosUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);

            return Task.FromResult(respuestaAutorizacion);
        }
        private void ValidarParametros(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            var mensaje = string.Empty;
            var valido = false;

            if (string.IsNullOrEmpty(nombreUsuario))
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "nombreUsuario");
            else if (string.IsNullOrEmpty(hashUsuario))
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "hashUsuario");
            else if (string.IsNullOrEmpty(idAplicacion))
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "idAplicacion");
            else if (string.IsNullOrEmpty(nombreServicio))
                mensaje = string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "nombreServicio");
            else
                valido = true;

            if (!valido)
                throw new ServiciosNegocioException(mensaje);
        }

        private static HttpResponseMessage ObtenerPermisosUsuario(string nombreUsuario, string hashUsuario, string idAplicacion,
                                                                  // ReSharper disable once UnusedParameter.Local
                                                                  string nombreServicio)
        {
            var respuesta = new HttpResponseMessage();

            if ((nombreUsuario != "jcastano@dnp.gov.co" && nombreUsuario != "jdelgado") && (!string.IsNullOrEmpty(hashUsuario)))
            {
                respuesta.StatusCode = HttpStatusCode.Unauthorized;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.UsuarioAutorizado;
            }
            else if (idAplicacion != "AP:PIIP" && idAplicacion != "AP:Backbone")
            {
                respuesta.StatusCode = HttpStatusCode.Unauthorized;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.AplicacionNoExiste;
            }
            else
            {
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;
            }

            return respuesta;
        }
    }
}
