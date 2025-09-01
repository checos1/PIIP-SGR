namespace DNP.ServiciosTransaccional.Web.API.Test.Configuracion
{
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.ModificacionLey;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Servicios.Interfaces;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites;
    using Mock;
    using Persistencia.Interfaces.Transferencias;
    using Servicios.Implementaciones.Transferencias;
    using Servicios.Implementaciones.Transversales;
    using Servicios.Interfaces.Proyectos;
    using Servicios.Interfaces.Transferencias;
    using Servicios.Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Autorizacion;
    using System.Diagnostics.CodeAnalysis;
    using Unity;

    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer();

                container.RegisterType<ITransferenciaPersistencia, TransferenciaServicioMock>()
                         .RegisterType<IAutorizacionUtilidades, AutorizacionUtilidadesMock>()
                         .RegisterType<ITransferenciaServicio, TransferenciaServicio>()
                         .RegisterType<IAuditoriaServicios, AuditoriaServicios>()
                         .RegisterType<ICreacionBpinServicio, CreacionBpinServicio>()
                         .RegisterType<ICreacionBpinPersistencia, CreacionBpinServicioMock>()
                         .RegisterType<IProyectoServicio, ProyectoServicioMock>()
                         .RegisterType<IBpinServicio, BpinServicioMock>()
                         .RegisterType<IMergeServicio, MergeServicioMock>()
                         .RegisterType<ITramiteServicio, TramiteServicioMock>()               
                         .RegisterType<IClienteHttpServicios, ClienteHttpServicios>()
                         .RegisterType<IModificacionLeyServicio, ModificacionLeyServicioMock>()
                    ;

                return container;
            }
        }
    }
}
