namespace DNP.ServiciosWBS.Web.API.App_Start
{
    using Microsoft.ApplicationInsights.Extensibility;
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class AppInsightsHelper
    {
        public static void Initialize()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];
            TelemetryConfiguration.Active.DisableTelemetry = !Convert.ToBoolean(ConfigurationManager.AppSettings["InstrumentationEnabled"]);
        }

    }
}