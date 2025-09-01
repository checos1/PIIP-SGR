
using DNP.ServiciosTransaccional.Web.API.Filters;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace DNP.ServiciosTransaccional.Web.API
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http.Cors;

    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            // Web API configuration and services
            //  config.SuppressDefaultHostAuthentication();
            // config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            config.Filters.Add(new ServiciosNegocioAutenticacionBasicaAttribute());
            config.Filters.Add(new ServiciosNegocioExceptionFilterAttribute());
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.MessageHandlers.Add(new LoggingHandler());

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());
            var enabledTracing = Convert.ToBoolean(ConfigurationManager.AppSettings["InstrumentationEnabled"]);
            if (enabledTracing)
            {
                config.EnableSystemDiagnosticsTracing();
            }
        }
    }
}
