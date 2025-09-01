namespace DNP.Backbone.Web.API
{
    using Microsoft.Practices.Unity;
    using System.Web.Http;
    using DNP.Backbone.Servicios.Implementaciones;
    using DNP.Backbone.Servicios.Interfaces;
    using Unity.WebApi;
    using DNP.Backbone.Servicios.Implementaciones.Cache;
    using Servicios.Implementaciones.Auditoria;
    using System.Diagnostics.CodeAnalysis;
    using DNP.Backbone.Servicios.Implementaciones.Flujos;
    using DNP.Backbone.Servicios.Implementaciones.ServiciosNegocio;
    using DNP.Backbone.Servicios.Implementaciones.AutorizacionNegocio;
    using Servicios.Implementaciones.Inbox;
    using Servicios.Interfaces.Auditoria;
    using Servicios.Interfaces.Autorizacion;
    using Servicios.Interfaces.Cache;
    using Servicios.Interfaces.ServiciosNegocio;
    using Servicios.Interfaces.Inbox;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Servicios.Implementaciones.Tramites;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Implementaciones.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.PowerBI;
    using DNP.Backbone.Servicios.Implementaciones.PowerBI;
    using DNP.Backbone.Servicios.Implementaciones.Monitoreo;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
    using DNP.Backbone.Servicios.Implementaciones.MensajesMantenimiento;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using DNP.Backbone.Servicios.Implementaciones.Consola;
    using DNP.Backbone.Servicios.Interfaces.Identidad;
    using DNP.Backbone.Servicios.Implementaciones.Identidad;
    using DNP.Backbone.Persistencia.Interfaces;
    using DNP.Backbone.Persistencia.Implementaciones;
    using DNP.Backbone.Servicios.Interfaces.Nivel;
    using DNP.Backbone.Servicios.Implementaciones.Nivel;
    using DNP.Backbone.Servicios.Implementaciones.Programacion;
    using DNP.Backbone.Servicios.Interfaces.Programacion;
    using DNP.Backbone.Servicios.Interfaces.CentroAyuda;
    using DNP.Backbone.Servicios.Implementaciones.CentroAyuda;
    using DNP.Backbone.Servicios.Interfaces.UsuarioNotificacion;
    using DNP.Backbone.Servicios.Implementaciones.UsuarioNotificacion;
    using DNP.Backbone.Servicios.Interfaces.Preguntas;
    using DNP.Backbone.Servicios.Implementaciones.Preguntas;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.Backbone.Servicios.Implementaciones.FuenteFinanciacion;
    using DNP.Backbone.Servicios.Interfaces.DatosAdicionales;
    using DNP.Backbone.Servicios.Implementaciones.DatosAdicionales;
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using DNP.Backbone.Servicios.Implementaciones.Focalizacion;
    using DNP.Backbone.Servicios.Implementaciones.SeguimientoControl;
    using DNP.Backbone.Servicios.Interfaces.SeguimientoControl;
    using DNP.Backbone.Servicios.Interfaces.Priorizacion;
    using DNP.Backbone.Servicios.Implementaciones.Priorizacion;
    using DNP.Backbone.Servicios.Implementaciones.SGR;
    using DNP.Backbone.Servicios.Interfaces.SGR;
    using DNP.Backbone.Servicios.Interfaces.Administracion;
    using DNP.Backbone.Servicios.Implementaciones.Administracion;
    using DNP.Backbone.Servicios.Interfaces.Catalogos;
    using DNP.Backbone.Servicios.Implementaciones.Catalogos;
    using DNP.Backbone.Web.API.KeyVault;

    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.Backbone.Servicios.Implementaciones.TramiteIncorporacion;
    using DNP.Backbone.Servicios.Interfaces.TramitesDistribucion;
    using DNP.Backbone.Servicios.Implementaciones.TramitesDistribucion;
    using DNP.Backbone.Servicios.Interfaces.ModificacionLey;
    using DNP.Backbone.Servicios.Implementaciones.ModificacionLey;
    using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
    using DNP.Backbone.Servicios.Implementaciones.ReportesPIIP;
    using DNP.Backbone.Servicios.Interfaces.ManejadorArchivos;
    using DNP.Backbone.Servicios.Implementaciones.ManejadorArchivos;
    using DNP.Backbone.Servicios.Interfaces.SGP;
    using DNP.Backbone.Servicios.Implementaciones.SGP;
    using DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad;
    using DNP.Backbone.Servicios.Implementaciones.SGP.AdministradorEntidad;
    using DNP.Backbone.Servicios.Implementaciones.SGP.Tramite;
    using DNP.Backbone.Servicios.Interfaces.SGP.Tramite;
    using DNP.Backbone.Servicios.Interfaces.Transversal;
    using DNP.Backbone.Servicios.Implementaciones.Transversal;

    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración.Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            UnityContainer container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IAuditoriaServicios, AuditoriaServicios>();
            container.RegisterType<IBackboneServicios, BackboneServicios>();
            container.RegisterType<IInboxServicios, InboxServicios>();
            container.RegisterType<IFlujoServicios, FlujoServicios>();
            container.RegisterType<IAutorizacionServicios, AutorizacionServicios>();
            container.RegisterType<IIdentidadServicios, IdentidadServicios>(); 
            container.RegisterType<ICacheEntidadesNegocioServicios, CacheEntidadesNegocioServicios>();
            container.RegisterType<IClienteHttpServicios, ClienteHttpServicios>();
            container.RegisterType<IServiciosNegocioServicios, ServiciosNegocioServicios>();
            container.RegisterType<ITramiteServicios, TramiteServicios>();
            container.RegisterType<IProyectoServicios, ProyectoServicios>();
            container.RegisterType<IDocumentoSoporteServicios, DocumentoSoporteServicios>();
            container.RegisterType<IEmbedServicios, EmbedServicios>();
            container.RegisterType<IAlertasConfigServicios, AlertasConfigServicios>();
            container.RegisterType<IMapColumnasServicios, MapColumnasServicios>();
            container.RegisterType<IAlertasGeneradasServicios, AlertasGeneradasServicios>();
            container.RegisterType<IMensajeMantenimientoServicio, MensajeMantenimientoServicio>();
            container.RegisterType<ICentroAyudaServicio, CentroAyudaServicio>();
            container.RegisterType<IConsolaTramiteServicios, ConsolaTramiteServicios>();
            container.RegisterType<IConsolaProyectosServicio, ConsolaProyectosServicio>();
            container.RegisterType<INivelServicios, NivelServicios>();
            container.RegisterType<IProgramacionServicios, ProgramacionServicios>();
            container.RegisterType<IUsuarioNotificacionConfigServicio, UsuarioNotificacionConfigServicio>();

            container.RegisterType<IAutorizacionPersistencia, AutorizacionPersistencia>();
            container.RegisterType<IMensajeNotificacionServicio, MensajeNotificacionServicio>();
            container.RegisterType<IPreguntasPersonalizadasServicios, PreguntasPersonalizadasServicios>();
            container.RegisterType<IFuenteFinanciacionServicios, FuenteFinanciacionServicio>();
            container.RegisterType<IDatosAdicionalesServicios, DatosAdicionalesServicio>();
            container.RegisterType<IProgramarSolicitadoServicio, ProgramarSolicitadoServicio>();
            container.RegisterType<IPoliticasTransversalesFuentesServicios, PoliticasTransversalesFuentesServicio>();
            container.RegisterType<IIndicadoresPolitica, IndicadoresPoliticaServicio>();
            container.RegisterType<ICategoriaProductosPoliticaServicios, CategoriaProductosPoliticaServicio>();
            container.RegisterType<IPoliticasTransversalesCrucePoliticasServicios, PoliticasCrucePoliticasServicio>();
            container.RegisterType<IDesagregarEdtServicio, DesagregarEdtServicio>();
            container.RegisterType<IProgramarActividadesServicio, ProgramarActividadesServicio>();
            container.RegisterType<IProgramarProductosServicio, ProgramarProductosServicio>();
            container.RegisterType<IGestionSeguimientoServicio, GestionSeguimientoServicio>();
            container.RegisterType<IFuentesAprobacionServicio, FuentesAprobacionServicio>();
            container.RegisterType<IPriorizacionServicios, PriorizacionServicios>();
            container.RegisterType<IEjecutorServicio, EjecutorServicio>();
            container.RegisterType<IAdministrarDocumentoServicio, AdministrarDocumentoServicio>();
            container.RegisterType<ICatalogoServicio, CatalogoServicio>();
            container.RegisterType<IReporteAvanceProductoServicio, ReporteAvanceProductoServicio>();
            container.RegisterType<IReporteAvanceRegionalizacionServicio, ReporteAvanceRegionalizacionServicio>();
            container.RegisterType<ITramiteIncorporacionServicios, TramiteIncorporacionServicio>();
            container.RegisterType<IModificacionLeyServicios, ModificacionLeyServicios>();
            container.RegisterType<IKeyVaultManager, KeyVaultManager>();
            container.RegisterType<IReportesPIIPServicio, ReportePIIPServicio>();
            container.RegisterType<IManejadorArchivosServicios, ManejadorArchivosServicios>();
            container.RegisterType<ISGRServicios, SGRServicios>();
            container.RegisterType<ISGRViabilidadServicios, SGRViabilidadServicios>();
            container.RegisterType<ISGPServicios, SGPServicios>();

            container.RegisterType<ISGRCtusServicios, SGRCtusServicios>();
            container.RegisterType<ISGPPreguntasPersonalizadasServicios, SGPPreguntasPersonalizadasServicios>();            
            container.RegisterType<ISGPViabilidadServicios, SGPViabilidadServicios>();
            container.RegisterType<IAdministradorEntidadSgpServicios, AdministradorEntidadSgpServicios>();
            container.RegisterType<ISGPTramiteServicios, SGPTramiteServicios>();
            container.RegisterType<ISGPTramiteProyectoServicios, SGPTramiteProyectoServicios>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}