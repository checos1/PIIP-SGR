using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.TramitesDistribucion;
using DNP.Backbone.Web.API.Controllers.TramitesDistribucion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using Microsoft.Practices.Unity;
using System;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class TramitesDistribucionControllerTest
    {
        private ITramitesDistribucionServicios _tramitesDistribucionServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private TramitesDistribucionController _tramitesDistribucionController;

        [TestInitialize]
        public void Init()
        {
            _tramitesDistribucionServicios = Config.UnityConfig.Container.Resolve<ITramitesDistribucionServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();

            _tramitesDistribucionController = new TramitesDistribucionController(_tramitesDistribucionServicios, _autorizacionServicios);
            _tramitesDistribucionController.ControllerContext.Request = new HttpRequestMessage();
            _tramitesDistribucionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramitesDistribucionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _tramitesDistribucionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerTramitesDistribucionAnteriores_Ok()
        {
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var actionResult = _tramitesDistribucionController.ObtenerTramitesDistribucionAnteriores(new Guid(instanciaId)).Result;
            Assert.IsNotNull(actionResult);
        }
    }
}
