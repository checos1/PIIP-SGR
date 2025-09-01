namespace DNP.ServiciosEnrutamiento.Web.API.Filters
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http.ExceptionHandling;
    using Microsoft.ApplicationInsights;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuraci�n.Se excluye de la cobertura porque este c�digo se autogenero con la instalaci�n de alguna librer�a y/o es una clase de configuraci�n para el funcionamiento de la aplicaci�n.
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