namespace DNP.Backbone.Web.API.Test.WebAPI.Tramite
{
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using DNP.Backbone.Servicios.Interfaces.SGP.Tramite;
    using DNP.Backbone.Web.API.Controllers.SGP.Tramite;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using System.Web.Http.Results;
    using DNP.Backbone.Comunes.Dto;

    [TestClass]
    public class SGPTramiteProyectoControllerTest
    {
        private ISGPTramiteProyectoServicios _tramiteProyectoSGPServicio;
        private IAutorizacionServicios _autorizacionServicios;
        private SGPTramiteProyectoController _tramiteProyectoSGPController;

        [TestInitialize]
        public void Init()
        {
            _tramiteProyectoSGPServicio = Config.UnityConfig.Container.Resolve<ISGPTramiteProyectoServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _tramiteProyectoSGPController = new SGPTramiteProyectoController(_tramiteProyectoSGPServicio, _autorizacionServicios);
            _tramiteProyectoSGPController.ControllerContext.Request = new HttpRequestMessage();
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _tramiteProyectoSGPController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }
    }
}
