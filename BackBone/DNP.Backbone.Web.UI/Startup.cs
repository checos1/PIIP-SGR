using DNP.Backbone.Web.UI;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace DNP.Backbone.Web.UI
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
