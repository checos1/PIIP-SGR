namespace DNP.Backbone.Web.API.Test.Config
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;
    using Mocks;
    using Servicios.Interfaces;
    using Servicios.Interfaces.Autorizacion;
    using Servicios.Interfaces.ServiciosNegocio;
    using Servicios.Interfaces.Inbox;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.Preguntas;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Servicios.Interfaces.PowerBI;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
    using DNP.Backbone.Servicios.Interfaces.Identidad;
    using DNP.Backbone.Servicios.Interfaces.Nivel;
    using DNP.Backbone.Servicios.Interfaces.Programacion;
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.Backbone.Servicios.Interfaces.Priorizacion;
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.Backbone.Servicios.Interfaces.TramitesDistribucion;
    using DNP.Backbone.Servicios.Interfaces.ModificacionLey;
    using DNP.Backbone.Servicios.Interfaces.SGR;
    using DNP.Backbone.Servicios.Interfaces.SGP;
    using DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad;
    using DNP.Backbone.Web.API.Test.Mocks.Tramite;
    using DNP.Backbone.Servicios.Interfaces.SGP.Tramite;

    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                UnityContainer container = new UnityContainer();
                container.RegisterType<IAutorizacionServicios, AutorizacionServiciosMock>();
                container.RegisterType<IIdentidadServicios, IdentidadServiciosMock>();                
                container.RegisterType<IBackboneServicios, BackboneServiciosMock>();
                container.RegisterType<IInboxServicios, InboxServiciosMock>();
                container.RegisterType<IFlujoServicios, FlujoServiciosMock>();
                container.RegisterType<ITramiteServicios, TramiteServiciosMock>();
                container.RegisterType<IProyectoServicios, ProyectoServiciosMock>();
                container.RegisterType<IServiciosNegocioServicios, ServiciosNegocioServiciosMock>();
                container.RegisterType<IMapColumnasServicios, MapColumnasServiciosMock>();
                container.RegisterType<IEmbedServicios, EmbedServiciosMock>();
                container.RegisterType<IAlertasConfigServicios, AlertasConfigServiciosMock>();
                container.RegisterType<IAlertasGeneradasServicios, AlertasGeneradasServiciosMock>();
                container.RegisterType<IConsolaTramiteServicios, ConsolaTramiteServiciosMock>();
                container.RegisterType<IMensajeMantenimientoServicio, MensajeMantenimientoServicioMock>();
                container.RegisterType<IConsolaProyectosServicio, ConsolaProyectoServiciosMock>();
                container.RegisterType<INivelServicios, NivelServiciosMock>();
                container.RegisterType<IProgramacionServicios, ProgramacionServiciosMock>();
                container.RegisterType<IIndicadoresPolitica, FocalizacionIndPoliticaServiciosMock>();
                container.RegisterType<ICategoriaProductosPoliticaServicios, FocalizacionCategoriaServiciosMock>();
                container.RegisterType<IFuentesAprobacionServicio, FuentesAprobacionServicioMock>();
                container.RegisterType<IPriorizacionServicios, PriorizacionServiciosMock>();
                container.RegisterType<ITramiteIncorporacionServicios, TramiteIncorporacionServiciosMock>();
                container.RegisterType<ITramitesDistribucionServicios, TramitesDistribucionMock>();
                container.RegisterType<IModificacionLeyServicios, ModificacionLeyServiciosMock>();
                container.RegisterType<ISGRViabilidadServicios, ViabilidadServiciosMock>();
                container.RegisterType<IPreguntasPersonalizadasServicios, PreguntasServiciosMock>();
                container.RegisterType<ISGPServicios, SGPServiciosMock>();
                container.RegisterType<ISGPViabilidadServicios, SGPViabilidadServiciosMock>();
                container.RegisterType<IAdministradorEntidadSgpServicios, AdministradorEntidadSgpMock>();
                container.RegisterType<ISGPTramiteServicios, SGPTramiteServiciosMock>();
                container.RegisterType<ISGPTramiteProyectoServicios, SGPTramiteProyectoServiciosMock>();

                return container;
            }
        }
    }
}
