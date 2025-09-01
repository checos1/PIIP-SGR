namespace DNP.Backbone.Web.API.Test.WebApi
{
    using System.Collections.Generic;
    using System.Web.Http.Results;
    using Controllers;
    using Dominio.Dto;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;

    [TestClass]
    public class NotificacionesControllerTest
    {
        private IBackboneServicios _backboneServicios;
        private NotificacionesController _notificacionesController;

        [TestInitialize]
        public void Init()
        {
            _backboneServicios = Config.UnityConfig.Container.Resolve<IBackboneServicios>();
            _notificacionesController = new NotificacionesController(_backboneServicios);
        }
        [TestMethod]
        public void CuandoEnvioUsuario_NoRetornaResultado()
        {
            var responsable = "xxxxxxx";

            var actionResult = _notificacionesController.ObtenerNotificacionesPorResponsable(responsable).Result;
            var proyectosPorResponsable = ((OkNegotiatedContentResult<List<NotificacionesDto>>)actionResult).Content;

            Assert.IsTrue(proyectosPorResponsable.Count == 0);
        }

        [TestMethod]
        public void CuandoEnvioUsuario_RetornaResultado()
        {
            var responsable = "jdelgado";

            var actionResult = _notificacionesController.ObtenerNotificacionesPorResponsable(responsable).Result;
            var proyectosPorResponsable = ((OkNegotiatedContentResult<List<NotificacionesDto>>)actionResult).Content;

            Assert.IsTrue(proyectosPorResponsable.Count > 0);
        }
    }
}