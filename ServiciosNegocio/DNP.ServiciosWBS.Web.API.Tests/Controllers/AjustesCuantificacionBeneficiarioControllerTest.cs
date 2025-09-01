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
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using Unity;

    [TestClass]
    public class AjustesCuantificacionBeneficiarioControllerTest
    {
        private IAjustesCuantificacionBeneficiarioServicios ajustesCuantificacionBeneficiarioServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjustesCuantificacionBeneficiarioController _ajustesCuantificacionBeneficiarioController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            ajustesCuantificacionBeneficiarioServicios = contenedor.Resolve<IAjustesCuantificacionBeneficiarioServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _ajustesCuantificacionBeneficiarioController = new AjustesCuantificacionBeneficiarioController(ajustesCuantificacionBeneficiarioServicios, AutorizacionUtilizades);
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request = new HttpRequestMessage();

            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "CD85FEF3-53CB-4ACD-B3F3-B1EE37E13B50");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "5D81AA9B-E14A-4158-8084-7D48BD39C224");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idFormulario", "5D81AA9B-E14A-4158-8084-7D48BD39C224");
            _ajustesCuantificacionBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioBpinValido()
        {
            var result = (OkNegotiatedContentResult<AjustesCuantificacionBeneficiarioDto>)await _ajustesCuantificacionBeneficiarioController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Vigencias.Count > 0);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioResultadoVacio()
        {
            var resultados = await _ajustesCuantificacionBeneficiarioController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioPreview()
        {
            var result = (OkNegotiatedContentResult<AjustesCuantificacionBeneficiarioDto>)await _ajustesCuantificacionBeneficiarioController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioGuardarTemporal_OK()
        {
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesCuantificacionBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "CD85FEF3-53CB-4ACD-B3F3-B1EE37E13B50");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "5D81AA9B-E14A-4158-8084-7D48BD39C224");

            var cuantificacionBeneficiario = new AjustesCuantificacionBeneficiarioDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesCuantificacionBeneficiarioController.Temporal(cuantificacionBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task CuantificacionBeneficiarioGuardarDefinitivo_OK()
        {
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesCuantificacionBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "CD85FEF3-53CB-4ACD-B3F3-B1EE37E13B50");
            _ajustesCuantificacionBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "5D81AA9B-E14A-4158-8084-7D48BD39C224");

            var cuantificacionBeneficiario = new AjustesCuantificacionBeneficiarioDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesCuantificacionBeneficiarioController.Definitivo(cuantificacionBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesCuantificacionBeneficiarioController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
