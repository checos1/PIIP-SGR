using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace DNP.ServiciosNegocio.Web.API.Test.Utilidades
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class ContextoApi
    {
        private const string Urlbase = "http://localhost:49686/";

        public static void CrearContextoApi<T>(string usuario, string permiso, string url, string nombreControlador, string accion, Dictionary<string, string> cabeceras, T controlador)
            where T : ApiController
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, Urlbase);
            var requestContext = new HttpRequestContext();
            var route = config.Routes.MapHttpRoute(nombreControlador, url);
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", accion } });
            controlador.ControllerContext = new HttpControllerContext(config, routeData, request);
            controlador.Request = request;
            controlador.RequestContext = requestContext;

            foreach (var cabecera in cabeceras)
            {
                controlador.Request.Headers.Add(cabecera.Key, cabecera.Value);
            }

            controlador.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(usuario, "Basic"), new[] { permiso });
        }
    }
}
