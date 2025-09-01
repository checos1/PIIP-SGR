
using System.Web.Http;

namespace DNP.ServiciosWBS.Web.API
{
    using System;
    using System.Configuration;
    using System.Web.Http.Cors;
    using System.Web.Http.ExceptionHandling;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using Filters;

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
            config.Filters.Add(new ValidationFilterAttribute());
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
