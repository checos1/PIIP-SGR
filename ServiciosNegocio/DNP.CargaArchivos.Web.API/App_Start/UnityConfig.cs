
namespace DNP.CargaArchivos.Web.API
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using Servicios.Implementaciones;
    using Servicios.Interfaces.CargaArchivo;
    using Unity;
    using Unity.WebApi;

    public class UnityConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ICargaArchivo, CargaArchivo>()
                     .RegisterType<IAutorizacionUtilidades, AutorizacionUtilidades>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}