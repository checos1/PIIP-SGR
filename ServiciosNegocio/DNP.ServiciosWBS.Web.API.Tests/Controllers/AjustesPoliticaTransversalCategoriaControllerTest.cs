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
    public class AjustesPoliticaTransversalCategoriaControllerTest
    {
        private IAjustesPoliticaTransversalCategoriaServicios AjustesPoliticaTransversalCategoriaServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjustesPoliticaTransversalCategoriaController _ajustesPoliticaTransversalCategoriaController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            AjustesPoliticaTransversalCategoriaServicios = contenedor.Resolve<IAjustesPoliticaTransversalCategoriaServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _ajustesPoliticaTransversalCategoriaController = new AjustesPoliticaTransversalCategoriaController(AjustesPoliticaTransversalCategoriaServicios, AutorizacionUtilizades);
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request = new HttpRequestMessage();

            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAccion", "A027C5CC-196F-4683-AEF9-B754B0083273");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "CDED65AA-EE09-46BB-9AA9-C91CA7AD80AA");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idFormulario", "CDED65AA-EE09-46BB-9AA9-C91CA7AD80AA");
            _ajustesPoliticaTransversalCategoriaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalCategoriaBpinValido()
        {
            var result = (OkNegotiatedContentResult<AjustesPoliticaTCategoriasDto>)await _ajustesPoliticaTransversalCategoriaController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Politicas.Count > 0);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalCategoriaResultadoVacio()
        {
            var resultados = await _ajustesPoliticaTransversalCategoriaController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalCategoriaPreview()
        {
            var result = (OkNegotiatedContentResult<AjustesPoliticaTCategoriasDto>)await _ajustesPoliticaTransversalCategoriaController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalCategoriaGuardarTemporal_OK()
        {
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticaTransversalCategoriaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAccion", "A027C5CC-196F-4683-AEF9-B754B0083273");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "CDED65AA-EE09-46BB-9AA9-C91CA7AD80AA");

            var politicaTransversalCategoria = new AjustesPoliticaTCategoriasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesPoliticaTransversalCategoriaController.Temporal(politicaTransversalCategoria);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task AjustesPoliticaTransversalCategoriaGuardarDefinitivo_OK()
        {
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesPoliticaTransversalCategoriaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAccion", "A027C5CC-196F-4683-AEF9-B754B0083273");
            _ajustesPoliticaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "CDED65AA-EE09-46BB-9AA9-C91CA7AD80AA");

            var politicaTransversalCategoria = new AjustesPoliticaTCategoriasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesPoliticaTransversalCategoriaController.Definitivo(politicaTransversalCategoria);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesPoliticaTransversalCategoriaController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
