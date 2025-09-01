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
    public class IncluirPoliticasControllerTest
    {
        private IIncluirPoliticasServicios IncluirPoliticasServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IncluirPoliticasController _incluirPoliticasController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            IncluirPoliticasServicios = contenedor.Resolve<IIncluirPoliticasServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _incluirPoliticasController = new IncluirPoliticasController(IncluirPoliticasServicios, AutorizacionUtilizades);
            _incluirPoliticasController.ControllerContext.Request = new HttpRequestMessage();

            _incluirPoliticasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAccion", "712FEC8A-B5AD-4F03-8CFF-228846D495BB");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "C6676732-5896-4B65-9B49-E51B908ED324");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idFormulario", "13E75985-8542-4C40-94A6-FAEDE568AA4C");
            _incluirPoliticasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task IncluirPoliticasBpinValido()
        {
            var result = (OkNegotiatedContentResult<IncluirPoliticasDto>)await _incluirPoliticasController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Politicas.Count > 0);
        }

        [TestMethod]
        public async Task IncluirPoliticasResultadoVacio()
        {
            var resultados = await _incluirPoliticasController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task IncluirPoliticasPreview()
        {
            var result = (OkNegotiatedContentResult<IncluirPoliticasDto>)await _incluirPoliticasController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task IncluirPoliticasGuardarTemporal()
        {
            _incluirPoliticasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _incluirPoliticasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAccion", "712FEC8A-B5AD-4F03-8CFF-228846D495BB");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "C6676732-5896-4B65-9B49-E51B908ED324");

            var incluirPoliticas = new IncluirPoliticasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _incluirPoliticasController.Temporal(incluirPoliticas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task IncluirPoliticasGuardarDefinitivo()
        {
            _incluirPoliticasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _incluirPoliticasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAccion", "712FEC8A-B5AD-4F03-8CFF-228846D495BB");
            _incluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "C6676732-5896-4B65-9B49-E51B908ED324");

            var incluirPoliticas = new IncluirPoliticasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _incluirPoliticasController.Definitivo(incluirPoliticas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _incluirPoliticasController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
