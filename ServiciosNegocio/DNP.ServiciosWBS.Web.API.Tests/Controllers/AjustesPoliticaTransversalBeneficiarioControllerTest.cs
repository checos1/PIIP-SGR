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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using Unity;

    [TestClass]
    public class AjustesPoliticaTransversalBeneficiarioControllerTest
    {
        private IAjustesPoliticaTransversalBeneficiarioServicios AjustesPoliticaTransversalBeneficiarioServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjustesPoliticaTransversalBeneficiarioController _ajustesPoliticaTransversalBeneficiarioController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            AjustesPoliticaTransversalBeneficiarioServicios = contenedor.Resolve<IAjustesPoliticaTransversalBeneficiarioServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _ajustesPoliticaTransversalBeneficiarioController = new AjustesPoliticaTransversalBeneficiarioController(AjustesPoliticaTransversalBeneficiarioServicios, AutorizacionUtilizades);
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request = new HttpRequestMessage();

            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "D8251078-90BC-4EC3-9496-114F128B694F");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idFormulario", "DBC7FE6A-91DF-4643-B1E4-F1971486E113");
            _ajustesPoliticaTransversalBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalBeneficiarioBpinValido()
        {
            var result = (OkNegotiatedContentResult<AjustesPoliticaTBeneficiarioDto>)await _ajustesPoliticaTransversalBeneficiarioController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Focalizacion_Beneficiarios_y_Recursos.Count > 0);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalBeneficiarioResultadoVacio()
        {
            var resultados = await _ajustesPoliticaTransversalBeneficiarioController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalBeneficiarioPreview()
        {
            var result = (OkNegotiatedContentResult<AjustesPoliticaTBeneficiarioDto>)await _ajustesPoliticaTransversalBeneficiarioController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalBeneficiarioGuardarTemporal_OK()
        {
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticaTransversalBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "D8251078-90BC-4EC3-9496-114F128B694F");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8");

            var ajustesPoliticaTransversalBeneficiario = new AjustesPoliticaTBeneficiarioDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesPoliticaTransversalBeneficiarioController.Temporal(ajustesPoliticaTransversalBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalBeneficiarioGuardarDefinitivo_OK()
        {
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticaTransversalBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "D8251078-90BC-4EC3-9496-114F128B694F");
            _ajustesPoliticaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8");

            var ajustesPoliticaTransversalBeneficiario = new AjustesPoliticaTBeneficiarioDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesPoliticaTransversalBeneficiarioController.Definitivo(ajustesPoliticaTransversalBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesPoliticaTransversalBeneficiarioController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
