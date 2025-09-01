

namespace DNP.EncabezadoPie.Web.API
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using Servicios.Implementaciones;
    using Servicios.Interfaces.EncabezadoPieBasico;
    using Persistencia.Interfaces.EncabezadoPie;
    using Persistencia.Implementaciones.EncabezadoPie;
    using Persistencia.Interfaces;
    using Persistencia.Implementaciones;
    using Unity;
    using Unity.WebApi;
    using Servicios.Interfaces.PriorizacionRecurso;
    using Servicios.Implementaciones.PriorizacionRecurso;
    using Persistencia.Interfaces.PriorizacionRecurso;
    using Persistencia.Implementaciones.PriorizacionRecurso;
    using Persistencia.Interfaces.Genericos;
    using Persistencia.Implementaciones.Genericos;
    using Servicios.Interfaces.DefinirAlcance;
    using Servicios.Implementaciones.DefinirAlcance;
    using Persistencia.Interfaces.DefinirAlcance;
    using Persistencia.Implementaciones.DefinirAlcance;

    public class UnityConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IEncabezadPieoBasicoServicio, EncabezadoPieServicio>()
                     .RegisterType<IAutorizacionUtilidades, AutorizacionUtilidades>()
                     .RegisterType<IContextoFactory, ContextoFactory>()
                     .RegisterType<IEncabezadoPiePersistencia, EncabezadoPiePersistencia>()
                     .RegisterType<IPriorizacionRecursoServicio, PriorizacionRecursoServicio>()
                     .RegisterType<IPriorizacionRecursoPersistencia, PriorizacionRecursoPersistencia>()
                     .RegisterType<IDefinirAlcanceServicio, DefinirAlcanceServicio>()
                     .RegisterType<IDefinirAlcancePersistencia, DefinirAlcancePersistencia>()
                     .RegisterType<IPersistenciaTemporal, PersistenciaTemporal>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}