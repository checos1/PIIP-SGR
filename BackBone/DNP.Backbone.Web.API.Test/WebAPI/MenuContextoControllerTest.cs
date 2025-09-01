namespace DNP.Backbone.Web.API.Test.WebApi
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;
    using Comunes.Dto;
    using Controllers;
    using Controllers.Flujos;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using Servicios.Interfaces.ServiciosNegocio;
    using Servicios.Interfaces.Inbox;

    [TestClass]
    public class MenuContextoControllerTest
    {

        private IFlujoServicios _flujoServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private MenuContextualController _menuContextualController;

        [TestInitialize]
        public void Init()
        {
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _menuContextualController = new MenuContextualController(_flujoServicios, _autorizacionServicios);
            _menuContextualController.ControllerContext.Request = new HttpRequestMessage();
            _menuContextualController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _menuContextualController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _menuContextualController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void MenuContextual_ObtenerFlujoPorInstanciaTarea_ParametrosRequestIncorrectos()
        {
            var nombreAplicacion = string.Empty;
            var usuarioDnp = string.Empty;
            var idInstancia = Guid.Empty;

            var actionResult = _menuContextualController.ObtenerFlujoPorInstanciaTarea(nombreAplicacion, usuarioDnp, idInstancia).Result;

            var content = ((OkNegotiatedContentResult<HttpStatusCode>)actionResult).Content;

            Assert.AreEqual(content, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void MenuContextual_ObtenerFlujoPorInstanciaTarea_ParametrosCorrectos()
        {
            var nombreAplicacion = "AP:Backbone";
            var usuarioDnp = "jdelgado";
            var idInstancia = Guid.Parse("24054986-A739-4990-A1C7-B662274873DF");

            var actionResult = _menuContextualController.ObtenerFlujoPorInstanciaTarea(nombreAplicacion, usuarioDnp, idInstancia).Result;

            var content = ((OkNegotiatedContentResult<FlujoMenuContextualDto>)actionResult).Content;

            Assert.IsNotNull(content);
        }
    }
}
