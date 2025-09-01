using DNP.ServiciosEnrutamiento.Persistencia.Implementaciones;
using DNP.ServiciosEnrutamiento.Persistencia.Interfaces;
using DNP.ServiciosEnrutamiento.Servicios.Implementaciones;
using DNP.ServiciosEnrutamiento.Servicios.Implementaciones.Transversales;
using DNP.ServiciosEnrutamiento.Servicios.Interfaces;
using DNP.ServiciosEnrutamiento.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace DNP.ServiciosEnrutamiento.Web.API
{
    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IContextoFactory, ContextoFactory>()
                     .RegisterType<IAuditoriaServicios, AuditoriaServicios>()
                     .RegisterType<IAutorizacionUtilidades, AutorizacionUtilidades>()
                     .RegisterType<ITallerPersistencia, TallerPersistencia>()
                     .RegisterType<ITallerServicio, TallerServicio>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}