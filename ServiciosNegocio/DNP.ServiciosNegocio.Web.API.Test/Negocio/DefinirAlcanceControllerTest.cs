namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
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
    using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
    using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using Unity;

    [TestClass]
    public class DefinirAlcanceControllerTest
    {
        private IDefinirAlcanceServicios DefinirAlcanceServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private DefinirAlcanceController _definirAlcanceController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            DefinirAlcanceServicios = contenedor.Resolve<IDefinirAlcanceServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _definirAlcanceController = new DefinirAlcanceController(DefinirAlcanceServicios, AutorizacionUtilizades);
            _definirAlcanceController.ControllerContext.Request = new HttpRequestMessage();

            _definirAlcanceController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idAccion", "42D3660A-8B88-4341-8FA9-7C45429C6A9A");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "A303CA4A-831D-4FA0-A74F-A39BFB962A15");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idFormulario", "61812836-A5BC-4ECD-B2E9-F3E2A7E13667");
            _definirAlcanceController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task DefinirAlcanceBpinValido()
        {
            var result = (OkNegotiatedContentResult<AlcanceDto>)await _definirAlcanceController.Consultar(Bpin);
            Assert.IsTrue(result.Content.DescripcionGeneralProyectoNueva.Count > 0);
        }

        [TestMethod]
        public async Task DefinirAlcanceResultadoVacio()
        {
            var resultados = await _definirAlcanceController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task DefinirAlcancePreview()
        {
            var result = (OkNegotiatedContentResult<AlcanceDto>)await _definirAlcanceController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task DefinirAlcanceGuardarTemporal()
        {
            _definirAlcanceController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _definirAlcanceController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idAccion", "42D3660A-8B88-4341-8FA9-7C45429C6A9A");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "A303CA4A-831D-4FA0-A74F-A39BFB962A15");

            var definirAlcance = new AlcanceDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _definirAlcanceController.Temporal(definirAlcance);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task DefinirAlcanceGuardarDefinitivo()
        {
            _definirAlcanceController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _definirAlcanceController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idAccion", "42D3660A-8B88-4341-8FA9-7C45429C6A9A");
            _definirAlcanceController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "A303CA4A-831D-4FA0-A74F-A39BFB962A15");

            var definirAlcance = new AlcanceDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _definirAlcanceController.Definitivo(definirAlcance);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _definirAlcanceController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
