using DNP.Backbone.Web.UI.Servicios.Interfaces;

namespace DNP.Backbone.Web.UI.Servicios.Implementaciones
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using Controllers;

    [ExcludeFromCodeCoverage]
    //La hoja de recursos no es posible accederla desde las pruebas unitarias. Por eso esta clase no puede ser testeada.
    public class OpcionesServicio : IOpcionesServicio
    {
        private readonly HomeController _homeController;

        public OpcionesServicio(HomeController homeController)
        {
            _homeController = homeController;

        }

        public void InicializarVariablesGlobales()
        {
            _homeController.ViewBag.ApiAuditoriaServicioBaseUri = ConfigurationManager.AppSettings["ApiAuditoria"];
            _homeController.ViewBag.ApiMensajeriaServicioBaseUri = ConfigurationManager.AppSettings["ApiMensajeria"];
            _homeController.ViewBag.ApiNotificacionServicioBaseUri = ConfigurationManager.AppSettings["ApiNotificacion"];
            _homeController.ViewBag.ApiIdentidadServicioBaseUri = ConfigurationManager.AppSettings["ApiIdentidad"];
            _homeController.ViewBag.ApiFormularioServicioBaseUri = ConfigurationManager.AppSettings["ApiPiipCore"];
            _homeController.ViewBag.ApiAutorizacionServicioBaseUri = ConfigurationManager.AppSettings["ApiAutorizacion"];
            _homeController.ViewBag.ApiFlujoServicioBaseUri = ConfigurationManager.AppSettings["ApiPiipCore"];
            _homeController.ViewBag.ApiBackboneServicioBaseUri = ConfigurationManager.AppSettings["ApiBackbone"];
            _homeController.ViewBag.NombreAplicacionBackbone = ConfigurationManager.AppSettings["NombreAplicacionBackbone"];
            _homeController.ViewBag.TopePaginacion = ConfigurationManager.AppSettings["TopePaginacion"];
            _homeController.ViewBag.IdTipoProyecto = ConfigurationManager.AppSettings["IdTipoProyecto"];
            _homeController.ViewBag.IdTipoTramite = ConfigurationManager.AppSettings["IdTipoTramite"];
            _homeController.ViewBag.IdTipoTramiteProgramacion = ConfigurationManager.AppSettings["IdTipoTramiteProgramacion"];
            _homeController.ViewBag.WebUIIdentidad = ConfigurationManager.AppSettings["ida:MetadataAddress"];
            _homeController.ViewBag.IdentidadCambioContrasena = ConfigurationManager.AppSettings["IdentidadCambioContrasena"];
            _homeController.ViewBag.ApiMotorFlujos= ConfigurationManager.AppSettings["ApiMotorFlujos"];
            _homeController.ViewBag.ApiArchivo = ConfigurationManager.AppSettings["ApiArchivo"];

            _homeController.ViewBag.ProyectosEstados = ConfigurationManager.AppSettings["ProyectosEstados"];
            _homeController.ViewBag.WebPDFBackbone = ConfigurationManager.AppSettings["pdf:Backbone"];
            _homeController.ViewBag.ApiCacheCatalogos = ConfigurationManager.AppSettings["ApiCacheCatalogos"];
            _homeController.ViewBag.ApiCache = ConfigurationManager.AppSettings["ApiCache"];
            _homeController.ViewBag.NombreEntorno = ConfigurationManager.AppSettings["NombreEntorno"];
            _homeController.ViewBag.AdministracionURL = ConfigurationManager.AppSettings["AdministracionURL"];
            _homeController.ViewBag.BackboneURL = ConfigurationManager.AppSettings["BackboneURL"];
            _homeController.ViewBag.ApiManejadorArchivos = ConfigurationManager.AppSettings["ApiManejadorArchivos"];
            _homeController.ViewBag.KeyCollection = ConfigurationManager.AppSettings["KeyCollection"];
            _homeController.ViewBag.InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];            
            _homeController.ViewBag.ApiFichasProyectosBaseUri = ConfigurationManager.AppSettings["ApiFichasProyectos"];
            _homeController.ViewBag.UrlPowerBIGantt = ConfigurationManager.AppSettings["PowerBIGantt"];
            _homeController.ViewBag.urlDiagramaActividades = ConfigurationManager.AppSettings["PowerBIActividades"];
        }
    }
}