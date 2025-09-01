
using System.Web.Http;

namespace DNP.ServiciosEnrutamiento.Web.API
{
    using DNP.ServiciosEnrutamiento.Web.API.Filters;
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http.Cors;
    using System.Web.Http.ExceptionHandling;

    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ServiciosEnrutamientoAutenticacionBasicaAttribute());
            config.Filters.Add(new ServiciosEnrutamientoExceptionFilterAttribute());
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
