using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using Microsoft.Practices.Unity;
using DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad;
using DNP.Backbone.Web.API.Controllers;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class AdministradorEntidadSgpControllerTest
    {
        private IAdministradorEntidadSgpServicios _administradorEntidadSgpServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private AdministradorEntidadSgpController _administradorEntidadSgpController;

        [TestInitialize]
        public void Init()
        {
            _administradorEntidadSgpServicios = Config.UnityConfig.Container.Resolve<IAdministradorEntidadSgpServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();

            _administradorEntidadSgpController = new AdministradorEntidadSgpController(_autorizacionServicios, _administradorEntidadSgpServicios);
            _administradorEntidadSgpController.ControllerContext.Request = new HttpRequestMessage();
            _administradorEntidadSgpController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _administradorEntidadSgpController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _administradorEntidadSgpController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerSectores_Ok()
        {
            var actionResult = _administradorEntidadSgpController.ObtenerSectores().Result;
            Assert.IsNotNull(actionResult);
        }
    }
}
