using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DNP.CargaArchivos.Web.API
{
    public class FilterConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
