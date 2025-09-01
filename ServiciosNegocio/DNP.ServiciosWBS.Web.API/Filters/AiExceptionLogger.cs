namespace DNP.ServiciosWBS.Web.API.Filters
{
    using Microsoft.ApplicationInsights;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http.ExceptionHandling;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class AiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception != null)
            {
                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}