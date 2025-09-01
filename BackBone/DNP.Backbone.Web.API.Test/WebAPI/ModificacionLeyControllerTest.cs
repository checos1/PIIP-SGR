using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.ModificacionLey;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.ModificacionLey;
using DNP.Backbone.Web.API.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.Http.Results;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class ModificacionLeyControllerTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private IModificacionLeyServicios _modificacionLeyServicios;
        private ModificacionLeyController _modificacionLeyController;

        [TestInitialize]
        public void Init()
        {
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _modificacionLeyServicios = Config.UnityConfig.Container.Resolve<IModificacionLeyServicios>();

            _modificacionLeyController = new ModificacionLeyController(_autorizacionServicios, _modificacionLeyServicios);
            _modificacionLeyController.ControllerContext.Request = new HttpRequestMessage();
            _modificacionLeyController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _modificacionLeyController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _modificacionLeyController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerInformacionPresupuestalMLEncabezado_Ok()
        {
            var actionResult = _modificacionLeyController.ObtenerInformacionPresupuestalMLEncabezado(204, 2297, "distribucion");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void ObtenerInformacionPresupuestalMLDetalle_Ok()
        {
            var actionResult = _modificacionLeyController.ObtenerInformacionPresupuestalMLDetalle(204, "distribucion");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void GuardarInformacionPresupuestalML_Ok()
        {
            var actionResult = _modificacionLeyController.GuardarInformacionPresupuestalML(new InformacionPresupuestalMLDto
            {
                TramiteProyectoId = 1,
                NivelId = "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0",
                SeccionCapitulo = 1
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }
    }
}
