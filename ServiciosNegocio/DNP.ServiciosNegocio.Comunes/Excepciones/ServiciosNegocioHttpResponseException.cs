namespace DNP.ServiciosNegocio.Comunes.Excepciones
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [ExcludeFromCodeCoverage]
    public class ServiciosNegocioHttpResponseException : HttpResponseException
    {
        public ServiciosNegocioHttpResponseException(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public ServiciosNegocioHttpResponseException(HttpStatusCode statusCode, string mensaje) : base(statusCode)
        {
            Response.ReasonPhrase = mensaje;
            Response.StatusCode = statusCode;
        }

        public ServiciosNegocioHttpResponseException(HttpResponseMessage response) : base(response)
        {
        }
    }
}
