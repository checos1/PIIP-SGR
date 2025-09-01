namespace DNP.ServiciosWBS.Web.API
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    [ExcludeFromCodeCoverage]
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}