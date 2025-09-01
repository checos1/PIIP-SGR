namespace DNP.Backbone.Comunes.Excepciones
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [ExcludeFromCodeCoverage]
    public class BackboneResponseException : HttpResponseException
    {
        public BackboneResponseException(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public BackboneResponseException(HttpStatusCode statusCode, string mensaje) : base(statusCode)
        {
            Response.ReasonPhrase = mensaje;
            Response.StatusCode = statusCode;
        }

        public BackboneResponseException(HttpResponseMessage response) : base(response)
        {
        }
    }
}
