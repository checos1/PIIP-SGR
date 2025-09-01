
namespace DNP.Backbone.Web.UI.Filters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Microsoft.ApplicationInsights;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext?.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {   
                    var ai = new TelemetryClient();
                    ai.TrackException(filterContext.Exception);
                } 
            }
            base.OnException(filterContext);
        }
    }
}