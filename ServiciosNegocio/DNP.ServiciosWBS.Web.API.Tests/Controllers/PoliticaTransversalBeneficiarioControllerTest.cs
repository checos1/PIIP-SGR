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
    public class PoliticaTransversalBeneficiarioControllerTest
    {
        private IPoliticaTransversalBeneficiarioServicios PoliticaTransversalBeneficiarioServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private PoliticaTransversalBeneficiarioController _politicaTransversalBeneficiarioController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            PoliticaTransversalBeneficiarioServicios = contenedor.Resolve<IPoliticaTransversalBeneficiarioServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _politicaTransversalBeneficiarioController = new PoliticaTransversalBeneficiarioController(PoliticaTransversalBeneficiarioServicios, AutorizacionUtilizades);
            _politicaTransversalBeneficiarioController.ControllerContext.Request = new HttpRequestMessage();

            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "D8251078-90BC-4EC3-9496-114F128B694F");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idFormulario", "DBC7FE6A-91DF-4643-B1E4-F1971486E113");
            _politicaTransversalBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioBpinValido()
        {
            var result = (OkNegotiatedContentResult<PoliticaTBeneficiarioDto>)await _politicaTransversalBeneficiarioController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Focalizacion_Beneficiarios_y_Recursos.Count > 0);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioResultadoVacio()
        {
            var resultados = await _politicaTransversalBeneficiarioController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioPreview()
        {
            var result = (OkNegotiatedContentResult<PoliticaTBeneficiarioDto>)await _politicaTransversalBeneficiarioController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarTemporal_OK()
        {
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _politicaTransversalBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "D8251078-90BC-4EC3-9496-114F128B694F");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8");

            var politicaTransversalBeneficiario = new PoliticaTBeneficiarioDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _politicaTransversalBeneficiarioController.Temporal(politicaTransversalBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarDefinitivo_OK()
        {
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _politicaTransversalBeneficiarioController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idAccion", "D8251078-90BC-4EC3-9496-114F128B694F");
            _politicaTransversalBeneficiarioController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8");

            var politicaTransversalBeneficiario = new PoliticaTBeneficiarioDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _politicaTransversalBeneficiarioController.Definitivo(politicaTransversalBeneficiario);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _politicaTransversalBeneficiarioController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
