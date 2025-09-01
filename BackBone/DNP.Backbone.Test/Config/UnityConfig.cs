namespace DNP.Backbone.Test.Config
{
    using Backbone.Servicios.Implementaciones;
    using Backbone.Servicios.Implementaciones.Auditoria;
    using Backbone.Servicios.Implementaciones.AutorizacionNegocio;
    using Backbone.Servicios.Implementaciones.Cache;
    using Backbone.Servicios.Implementaciones.Flujos;
    using Backbone.Servicios.Implementaciones.Inbox;
    using Backbone.Servicios.Interfaces;
    using Backbone.Servicios.Interfaces.Auditoria;
    using Backbone.Servicios.Interfaces.Autorizacion;
    using Backbone.Servicios.Interfaces.Cache;
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Backbone.Servicios.Interfaces.Inbox;
    using Microsoft.Practices.Unity;
    using Mocks;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Servicios.Implementaciones.Tramites;
    using DNP.Backbone.Servicios.Interfaces.PowerBI;
    using DNP.Backbone.Servicios.Implementaciones.PowerBI;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
    using DNP.Backbone.Servicios.Interfaces.Nivel;
    using DNP.Backbone.Servicios.Interfaces.Programacion;
    using DNP.Backbone.Servicios.Interfaces.CentroAyuda;
    using DNP.Backbone.Servicios.Implementaciones.CentroAyuda;
    using DNP.Backbone.Persistencia.Interfaces;
    using DNP.Backbone.Persistencia.Implementaciones;
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
    using DNP.Backbone.Servicios.Implementaciones.ReportesPIIP;
    using DNP.Backbone.Servicios.Interfaces.ModificacionLey;
    using DNP.Backbone.Servicios.Interfaces.SGR;
    using DNP.Backbone.Servicios.Implementaciones.SGR;
    using DNP.Backbone.Servicios.Interfaces.SGP;
    using DNP.Backbone.Servicios.Implementaciones.SGP;

    public static class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                UnityContainer container = new UnityContainer();
                container.RegisterType<IAuditoriaServicios, AuditoriaServicios>();
                container.RegisterType<IAutorizacionServicios, AutorizacionServicios>();
                container.RegisterType<IAutorizacionPersistencia, AutorizacionPersistencia>();
                container.RegisterType<ICacheEntidadesNegocioServicios, CacheEntidadesNegocioServicios>();
                container.RegisterType<IClienteHttpServicios, ClienteHttpServiciosMock>();
                container.RegisterType<IBackboneServicios, BackboneServicios>();
                container.RegisterType<IInboxServicios, InboxServicios>();
                container.RegisterType<IFlujoServicios, FlujoServicios>();
                container.RegisterType<ITramiteServicios, TramiteServicios>();
                container.RegisterType<ITramiteServicios, TramiteServicios>();
                container.RegisterType<IEmbedServicios, EmbedServiciosMock>();
                container.RegisterType<IServiciosNegocioServicios, ServiciosNegocioServiciosMock>();
                container.RegisterType<IProyectoServicios, ProyectoServiciosMock>();
                container.RegisterType<IAlertasConfigServicios, AlertasConfigServiciosMock>();
                container.RegisterType<IAlertasGeneradasServicios, AlertasGeneradasServiciosMock>();
                container.RegisterType<IMapColumnasServicios, MapColumnasServiciosMock>();
                container.RegisterType<IInboxServicios, InboxServiciosMock>();
                container.RegisterType<IConsolaTramiteServicios, ConsolaTramiteServiciosMock>();
                container.RegisterType<IMensajeMantenimientoServicio, MensajeMantenimientoServicioMock>();
                container.RegisterType<ICentroAyudaServicio, CentroAyudaServicio>();
                container.RegisterType<IConsolaProyectosServicio, ConsolaProyectoServiciosMock>();
                container.RegisterType<INivelServicios, NivelServiciosMock>();
                container.RegisterType<IProgramacionServicios, ProgramacionServiciosMock>();
                container.RegisterType<IIndicadoresPolitica, FocalizacionIndPoliticaServiciosMock>();
                container.RegisterType<ICategoriaProductosPoliticaServicios, FocalizacionCategoriaServiciosMock>();
                container.RegisterType<ITramiteIncorporacionServicios, TramiteIncorporacionServicioMock>();
                container.RegisterType<IModificacionLeyServicios, ModificacionLeyServiciosMock>();
                container.RegisterType<IReportesPIIPServicio, ReportePIIPServicio>();
                container.RegisterType<ISGRViabilidadServicios, SGRViabilidadServicios>();
                container.RegisterType<ISGPViabilidadServicios, SGPViabilidadServicios>();

                return container;
            }
        }
    }
}
