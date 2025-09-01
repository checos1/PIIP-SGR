using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace DNP.ServiciosEnrutamiento.Web.API
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Optimization;

    [ExcludeFromCodeCoverage]
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AppInsightsHelper.Initialize();
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}