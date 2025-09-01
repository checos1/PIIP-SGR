namespace DNP.ServiciosTransaccional.Web.API
{
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Comunes.Interfaces;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Transferencias;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Proyecto;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Transferencias;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Transversales;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;
    using Persistencia.Interfaces.Transferencias;
    using Persistencia.Interfaces.Transversales;
    using Servicios.Implementaciones.Proyectos;
    using Servicios.Interfaces.Proyectos;
    using Servicios.Interfaces.Transferencias;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;
    using Unity;
    using Unity.WebApi;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Servicios.Interfaces;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Fichas;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Priorizacion;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Priorizacion;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Programacion;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Programacion;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Programacion;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Programacion;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.ModificacionLey;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.ModificacionLey;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.ModificacionLey;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.ManejadorArchivos;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.ManejadorArchivos;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Transversales;

    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        [ExcludeFromCodeCoverage]
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IContextoFactory, ContextoFactory>()
                     .RegisterType<IAccionUtilidades, AccionUtilidades>()
                     .RegisterType<IAuditoriaServicios, AuditoriaServicios>()
                     .RegisterType<ICacheServicio, CacheServicio>()
                     .RegisterType<IAutorizacionUtilidades, AutorizacionUtilidades>()
                     .RegisterType<ITransferenciaServicio, TransferenciaServicio>()
                     .RegisterType<ITransferenciaPersistencia, TransferenciaPersistencia>()
                     .RegisterType<ICreacionBpinServicio, CreacionBpinServicio>()
                     .RegisterType<ICreacionBpinPersistencia, CreacionBpinPersistencia>()
                     .RegisterType<IProyectoPersistencia, ProyectoPersistencia>()
                     .RegisterType<IProyectoServicio, ProyectoServicio>()
                     .RegisterType<IBpinPersistencia, BpinPersistencia>()
                     .RegisterType<IBpinServicio, BpinServicio>()
                     .RegisterType<IMergePersistencia, MergePersistencia>()
                     .RegisterType<IMergeServicio, MergeServicio>()
                     .RegisterType<ITramiteServicio, TramiteServicios>()
                     .RegisterType<ITramitePersistencia, TramitePersistencia>()
                     .RegisterType<IClienteHttpServicios, ClienteHttpServicios>()
                     .RegisterType<IFichaServicios, FichaServicios>()
                     .RegisterType<IPriorizacionServicio, PriorizacionServicio>()
                     .RegisterType<IProgramacionServicio, ProgramacionServicio>()
                     .RegisterType<IProgramacionPersistencia, ProgramacionPersistencia>()
                     .RegisterType<IModificacionLeyServicio, ModificacionLeyServicio>()
                     .RegisterType<IModificacionLeyPersistencia, ModificacionLeyPersistencia>()
                     .RegisterType<IProyectoSGRPersistencia, ProyectoSGRPersistencia>()
                     .RegisterType<IBpinSGRPersistencia, BpinSGRPersistencia>()
                     .RegisterType<IContextoFactorySGR, ContextoFactorySGR>()
                     .RegisterType<IManejadorArchivosServicio, ManejadorArchivosServicio>()
                     .RegisterType<IParametrosPersistencia, ParametrosPersistencia>()
                     .RegisterType<IParametrosServicio, ParametrosServicio>()
                    ;

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

        }
    }
}
