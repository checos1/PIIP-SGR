namespace DNP.Backbone.Web.API
{
    using System.Web.Http;
    using Filters;
    using System;
    using System.Configuration;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Cors;
    using System.Diagnostics.CodeAnalysis;
    using Swashbuckle.Application;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
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

            config.Routes.MapHttpRoute(
                name: "swagger_root",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            config.Filters.Add(new BackboneExceptionFilterAttribute());
            config.Filters.Add(new IdentidadAutenticacionBasicaAttribute());
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
