using System.Diagnostics.CodeAnalysis;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DNP.Backbone.Web.API.Startup))]

namespace DNP.Backbone.Web.API
{
    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
