namespace DNP.Backbone.Web.UI
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("SignOut", "Account/SignOut", new { controller = "Account", action = "SignOut" });

            routes.MapRoute("Inbox", "Inbox/ProyectosPDF", new { controller = "Inbox", action = "ProyectosPDF" });

            routes.MapRoute("SignB2B", "Account/SignInB2B", new { controller = "Account", action = "SignInB2B" });
            routes.MapRoute("SignInMulti", "Account/SignInMulti", new { controller = "Account", action = "SignInMulti" });
            routes.MapRoute("SignOutCallback", "Account/SignOutCallback", new { controller = "Account", action = "SignOutCallback" });
            routes.MapRoute("ObtenerTokenAutorizacion", "Account/ObtenerTokenAutorizacion", new { controller = "Account", action = "ObtenerTokenAutorizacion" });
            routes.MapRoute("ValidarTokenAutorizacion", "Account/ValidarTokenAutorizacion", new { controller = "Account", action = "ValidarTokenAutorizacion" });
            routes.MapRoute("SwitchSignShared", "Account/SwitchSignShared", new { controller = "Account", action = "SwitchSignShared" });
            routes.MapRoute("SignShared", "Account/SignShared", new { controller = "Account", action = "SignShared" });

            routes.MapRoute("GetToken", "Home/GetToken", new { controller = "Home", action = "GetToken" });
            routes.MapRoute("", "Home/ObtenerExcel", new { controller = "Home", action = "ObtenerExcel" });
            routes.MapRoute("", "Home/ProyectosPDF", new { controller = "Home", action = "ProyectosPDF" });
            routes.MapRoute("", "PDF/ProyectosPDF", new { controller = "PDF", action = "ProyectosPDF" });
            routes.MapRoute("", "PDF/ProyectosPDFConsola", new { controller = "PDF", action = "ProyectosPDFConsola" });
            routes.MapRoute("", "PDF/ConsolaProyectosPDF", new { Controller = "PDF", action = "ConsolaProyectosPDF" });
            routes.MapRoute("", "PDF/TramitesPDF", new { controller = "PDF", action = "TramitesPDF" });
            routes.MapRoute("", "PDF/TramitesProyectosPDF", new { controller = "PDF", action = "TramitesProyectosPDF" });
            routes.MapRoute("", "PDF/ConsolaMonitoreoProyectosPDF", new { controller = "PDF", action = "ConsolaMonitoreoProyectosPDF" });
            routes.MapRoute("", "PDF/NotificacionesMantenimiento", new { controller = "PDF", action = "NotificacionesMantenimiento" });
            routes.MapRoute("", "PDF/TemarioCentroAyuda", new { controller = "PDF", action = "TemarioCentroAyuda" });
            routes.MapRoute("", "PDF/VideosCentroAyuda", new { controller = "PDF", action = "VideosCentroAyuda" });
            routes.MapRoute("", "PDF/PerfilesPDF", new { controller = "PDF", action = "PerfilesPDF" });
            routes.MapRoute("", "PDF/RolesPDF", new { controller = "PDF", action = "RolesPDF" });
            routes.MapRoute("", "PDF/InflexibilidadesPDF", new { controller = "PDF", action = "InflexibilidadesPDF" });
            routes.MapRoute("", "PDF/EntidadesPDF", new { controller = "PDF", action = "EntidadesPDF" });
            routes.MapRoute("", "PDF/ConsolaAlertaConfigPDF", new { controller = "PDF", action = "ConsolaAlertaConfigPDF" });
            routes.MapRoute("", "PDF/ConsolaTramitesPDF", new { controller = "PDF", action = "ConsolaTramitesPDF" });
            routes.MapRoute("", "PDF/ConsolaTramitesProyectosPDF", new { controller = "PDF", action = "ConsolaTramitesProyectosPDF" });
            routes.MapRoute("", "PDF/UsuariosPDF", new { controller = "PDF", action = "UsuariosPDF" });
            routes.MapRoute("", "PDF/InstanciasLogPDF", new { controller = "PDF", action = "InstanciasLogPDF" });
            routes.MapRoute("", "PDF/NotificacionesMensajesPDF", new { controller = "PDF", action = "NotificacionesMensajesPDF" });
            routes.MapRoute("GetTokenChangePassword", "Home/GetTokenChangePassword", new { controller = "Home", action = "GetTokenChangePassword" });

            routes.MapRoute("Default", "{*anything}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

        }
    }
}
