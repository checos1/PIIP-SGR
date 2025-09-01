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
    public class AjustesPoliticasTransversalesMetasControllerTest
    {
        private IAjustesPoliticasTransversalesMetasServicios AjustesPoliticasTransversalesMetasServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjustesPoliticasTransversalesMetasController _ajustesPoliticasTransversalesMetasController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            AjustesPoliticasTransversalesMetasServicios = contenedor.Resolve<IAjustesPoliticasTransversalesMetasServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _ajustesPoliticasTransversalesMetasController = new AjustesPoliticasTransversalesMetasController(AutorizacionUtilizades, AjustesPoliticasTransversalesMetasServicios);
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request = new HttpRequestMessage();

            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAccion", "B3C505D2-573D-455D-BB9F-7D7390A68CB6");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "FF02D036-FD30-4295-A760-DE10C2B25F4C");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idFormulario", "FF02D036-FD30-4295-A760-DE10C2B25F4C");
            _ajustesPoliticasTransversalesMetasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustesPoliticasTransversalesMetasBpinValido()
        {
            var result = (OkNegotiatedContentResult<AjustesPoliticaTMetasDto>)await _ajustesPoliticasTransversalesMetasController.Consultar(Bpin);
            Assert.IsTrue(result.Content.POLITICAS.Count > 0);
        }

        [TestMethod]
        public async Task AjustesPoliticasTransversalesMetasResultadoVacio()
        {
            var resultados = await _ajustesPoliticasTransversalesMetasController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task ObtenerAjustesPoliticasTransversalesPreview()
        {
            var result = (OkNegotiatedContentResult<AjustesPoliticaTMetasDto>)await _ajustesPoliticasTransversalesMetasController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task AjustesPoliticasTransversalesMetasGuardarTemporal_OK()
        {
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticasTransversalesMetasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAccion", "B3C505D2-573D-455D-BB9F-7D7390A68CB6");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "FF02D036-FD30-4295-A760-DE10C2B25F4C");

            var politicaTransversalMetas = new AjustesPoliticaTMetasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesPoliticasTransversalesMetasController.Temporal(politicaTransversalMetas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task AjustesPoliticasTransversalesMetasGuardarDefinitivo_OK()
        {
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticasTransversalesMetasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAccion", "B3C505D2-573D-455D-BB9F-7D7390A68CB6");
            _ajustesPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "FF02D036-FD30-4295-A760-DE10C2B25F4C");

            var politicaTransversalMetas = new AjustesPoliticaTMetasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesPoliticasTransversalesMetasController.Definitivo(politicaTransversalMetas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesPoliticasTransversalesMetasController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
