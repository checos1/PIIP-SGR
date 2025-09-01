namespace DNP.EncabezadoPie.Web.API.Test.Mock
{
    using ServiciosNegocio.Comunes.Autorizacion;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ServiciosNegocio.Comunes;

    [ExcludeFromCodeCoverage]
    public class AutorizacionUtilidadesMock : IAutorizacionUtilidades
    {
        public Task<HttpResponseMessage> ValidarUsuario(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            ValidarParametros(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);

            var respuestaAutorizacion = ObtenerPermisosUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);

            return Task.FromResult(respuestaAutorizacion);
        }

        private static HttpResponseMessage ObtenerPermisosUsuario(string nombreUsuario, string hashUsuario, string idAplicacion,
                                                               // ReSharper disable once UnusedParameter.Local
                                                               string nombreServicio)
        {
            var respuesta = new HttpResponseMessage();

            if ((nombreUsuario != "jdelgado" && nombreUsuario != "Jdelgado") && (!string.IsNullOrEmpty(hashUsuario)))
            {
                respuesta.StatusCode = HttpStatusCode.Unauthorized;
                respuesta.ReasonPhrase = CargaArchivo.UsuarioAutorizado;
            }
            else if (idAplicacion != "AP:PIIP" && idAplicacion != "AP:Administracion")
            {
                respuesta.StatusCode = HttpStatusCode.Unauthorized;
                respuesta.ReasonPhrase = CargaArchivo.AplicacionNoExiste;
            }
            else
            {
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = CargaArchivo.PostExitoso;
            }

            return respuesta;
        }

        private void ValidarParametros(string nombreUsuario, string hashUsuario, string idAplicacion, string nombreServicio)
        {
            var mensaje = string.Empty;
            var valido = false;

            if (string.IsNullOrEmpty(nombreUsuario))
                mensaje = string.Format(CargaArchivo.ErrorCamposObligatorios, "nombreUsuario");
            else if (string.IsNullOrEmpty(hashUsuario))
                mensaje = string.Format(CargaArchivo.ErrorCamposObligatorios, "hashUsuario");
            else if (string.IsNullOrEmpty(idAplicacion))
                mensaje = string.Format(CargaArchivo.ErrorCamposObligatorios, "idAplicacion");
            else if (string.IsNullOrEmpty(nombreServicio))
                mensaje = string.Format(CargaArchivo.ErrorCamposObligatorios, "nombreServicio");
            else
                valido = true;

            if (!valido)
                throw new Exception(mensaje);
        }
    }
}
