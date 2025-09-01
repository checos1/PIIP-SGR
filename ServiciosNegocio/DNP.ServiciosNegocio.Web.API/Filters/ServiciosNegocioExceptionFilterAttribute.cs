namespace DNP.ServiciosNegocio.Web.API.Filters
{
    using Servicios.Interfaces.Transversales;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http.Filters;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class ServiciosNegocioExceptionFilterAttribute : ExceptionFilterAttribute
    {



        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                return;
            }
            try
            {
                var auditoriaServicios = (IAuditoriaServicios)actionExecutedContext.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IAuditoriaServicios));
                auditoriaServicios.RegistrarErrorAuditoria(actionExecutedContext.Exception,
                    UtilidadesApi.GetClientIp(actionExecutedContext.Request),
                    ConfigurationManager.AppSettings["UsuarioGenericoServiciosNegocio"]);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }
    }
}