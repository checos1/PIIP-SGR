using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DNP.Backbone.Comunes.Utilidades.AutorizacionAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class MenuAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _menu;

        public MenuAuthorizeAttribute(string menu)
        {
            _menu = menu;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorized = base.IsAuthorized(actionContext);
            if (!authorized)
            {
                return false;
            }

            var menus = ClaimIdentityUtilidades.ObtenerMenus(actionContext.RequestContext.Principal);

            return menus.Any(x => x == _menu);
        }
    }
}
