namespace DNP.ServiciosWBS.Web.API.Tests.Controllers
{

    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using API.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using Unity;

    [TestClass]
    public class CuantificacionBeneficiarioControllerTest
    {
        private ICuantificacionBeneficiarioServicios CuantificacionBeneficiarioServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private CuantificacionBeneficiarioController _cuantificacionBeneficiarioController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            CuantificacionBeneficiarioServicios = contenedor.Resolve<ICuantificacionBeneficiarioServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _cuantificacionBeneficiarioController = new CuantificacionBeneficiarioController(CuantificacionBeneficiarioServicios, AutorizacionUtilizades);
            _cuantificacionBeneficiarioController.ControllerContext.Request = new HttpRequestMessage();

            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1CC3A855-12F1-4113-A044-014886298AA3");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idFormulario", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _cuantificacionBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioBpinValido()
        {
            var result = (OkNegotiatedContentResult<PoblacionDto>)await _cuantificacionBeneficiarioController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Vigencias.Count > 0);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioResultadoVacio()
        {
            var resultados = await _cuantificacionBeneficiarioController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioPreview()
        {
            var result = (OkNegotiatedContentResult<PoblacionDto>)await _cuantificacionBeneficiarioController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioGuardarTemporal_OK()
        {
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _cuantificacionBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1CC3A855-12F1-4113-A044-014886298AA3");

            var cuantificacionBeneficiario = new PoblacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cuantificacionBeneficiarioController.Temporal(cuantificacionBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioGuardarDefinitivo_OK()
        {
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _cuantificacionBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _cuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1CC3A855-12F1-4113-A044-014886298AA3");

            var cuantificacionBeneficiario = new PoblacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cuantificacionBeneficiarioController.Definitivo(cuantificacionBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _cuantificacionBeneficiarioController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
