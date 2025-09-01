namespace DNP.ServiciosNegocio.Web.API
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.ServiceModel.Channels;
    using System.Web;

    [ExcludeFromCodeCoverage]
    //Se excluye por ser una clase de comunicación con una base de datos o un servicio Externo. Este tipo de clases no permite la creación de una prueba unitaria sin adiciona complejidad en la inyección del código y la generación de los Mocks.
    public class UtilidadesApi
    {
        public static string GetClientIp(HttpRequestMessage request = null)
        {
            if (request?.Properties.ContainsKey("MS_HttpContext") == true)
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (!request?.Properties.ContainsKey(RemoteEndpointMessageProperty.Name) == true)
            {
                return null;
            }
            if (request != null)
            {
                var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return "::1";
        }
    }
}