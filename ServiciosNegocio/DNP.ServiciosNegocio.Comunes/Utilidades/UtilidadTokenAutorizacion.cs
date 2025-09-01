namespace DNP.ServiciosNegocio.Comunes.Utilidades
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http.Headers;

    [ExcludeFromCodeCoverage]
    //Se excluye al ser una utilidad en general. Las utlidades no tienen métodos para ser testeados.

    public static class UtilidadTokenAutorizacion
    {
        public static string ExtraerUsuario(string tokenAutorizacion)
        {
            if (!string.IsNullOrWhiteSpace(tokenAutorizacion))
            {
                AuthenticationHeaderValue header = AuthenticationHeaderValue.Parse(tokenAutorizacion);
                var base64BytesCodificados = System.Convert.FromBase64String(header.Parameter);
                var textoDescodificado = System.Text.Encoding.UTF8.GetString(base64BytesCodificados);
                char[] separador = { ':' };
                var usuario = textoDescodificado.Split(separador);
                return usuario[0];
            }
            return tokenAutorizacion;

        }
    }
}
