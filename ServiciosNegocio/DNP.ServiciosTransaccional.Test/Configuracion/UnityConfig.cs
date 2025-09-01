namespace DNP.ServiciosTransaccional.Test.Configuracion
{
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Fichas;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.ManejadorArchivos;
    using DNP.ServiciosTransaccional.Servicios.Interfaces;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.ManejadorArchivos;
    using Mock;
    using Persistencia.Interfaces.Proyecto;
    using Persistencia.Interfaces.Transferencias;
    using ServiciosTransaccional.Servicios.Implementaciones.Proyectos;
    using ServiciosTransaccional.Servicios.Implementaciones.Transferencias;
    using ServiciosTransaccional.Servicios.Implementaciones.Tramites;
    using ServiciosTransaccional.Servicios.Interfaces.Proyectos;
    using ServiciosTransaccional.Servicios.Interfaces.Transferencias;
    using ServiciosTransaccional.Servicios.Interfaces.Transversales;
    using ServiciosTransaccional.Servicios.Interfaces.Tramites;
    using Unity;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Transversales;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones;

    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer();

                container.RegisterType<IAuditoriaServicios, AuditoriaServicioMock>()
                         .RegisterType<ITransferenciaServicio, TransferenciaServicio>()
                         .RegisterType<ITransferenciaPersistencia, TransferenciaPersistenciaMock>()
                         .RegisterType<ICreacionBpinServicio, CreacionBpinServicio>()
                         .RegisterType<ICreacionBpinPersistencia, CreacionBpinPersistenciaMock>()
                         .RegisterType<IProyectoPersistencia, ProyectoPersistenciaMock>()
                         .RegisterType<IProyectoSGRPersistencia, ProyectoSGRPersistenciaMock>()
                         .RegisterType<IProyectoServicio, ProyectoServicio>()
                         .RegisterType<IBpinPersistencia, BpinPersistenciaMock>()
                         .RegisterType<IBpinSGRPersistencia, BpinSGRPersistenciaMock>()
                         .RegisterType<IBpinServicio, BpinServicio>()
                         .RegisterType<IMergePersistencia, MergePersistenciaMock>()
                         .RegisterType<IMergeServicio, MergeServicio>()
                         .RegisterType<ITramitePersistencia, TramitePersistenciaMock>()
                         .RegisterType<IClienteHttpServicios, ClienteHttpServicios>()
                         .RegisterType<ITramiteServicio, TramiteServicios>()
                         .RegisterType<IFichaServicios, FichaServicios>()
                         .RegisterType<IManejadorArchivosServicio, ManejadorArchivosServicio>()
                         .RegisterType<IParametrosPersistencia, ParametrosPersistencia>()
                         .RegisterType<IContextoFactory, ContextoFactory>()
                    ;

                return container;
            }
        }
    }
}
