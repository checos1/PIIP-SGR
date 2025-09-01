
namespace DNP.CargaArchivos.Web.API
{
    using System;
    using System.Configuration;
    using System.Web.Http.Cors;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Filters;
    using System.Diagnostics.CodeAnalysis;

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

            config.Filters.Add(new CargaArchivoAutenticacionBasicaAttribute());
            config.Filters.Add(new CargaArchivoExceptionFilterAttribute());
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());
            var enabledTracing = Convert.ToBoolean(ConfigurationManager.AppSettings["InstrumentationEnabled"]);
            if (enabledTracing)
            {
                config.EnableSystemDiagnosticsTracing();
            }
        }
    }
}
