[assembly: WebActivator.PreApplicationStartMethod(typeof(DNP.Backbone.Web.UI.App_Start.Code52_i18n), "Start")]
namespace DNP.Backbone.Web.UI.App_Start {

    using System.Web.Mvc;
    using System.Web.Routing;
    using Code52.i18n;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    // Autogenerado o de Configuraci�n.
    public class Code52_i18n
    {
        public static void Start()
        {
            RouteTable.Routes.MapRoute("Language", "i18n/Code52_i18n_language/{id}", new { controller = "Language", action = "Language", id = UrlParameter.Optional });
            GlobalFilters.Filters.Add(new LanguageFilterAttribute());
        }
    }
}
