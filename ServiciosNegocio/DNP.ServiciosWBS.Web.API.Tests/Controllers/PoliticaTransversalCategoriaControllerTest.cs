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
    public class PoliticaTransversalCategoriaControllerTest
    {
        private IPoliticaTransversalCategoriaServicios PoliticaTransversalCategoriaServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private PoliticaTransversalCategoriaController _politicaTransversalCategoriaController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            PoliticaTransversalCategoriaServicios = contenedor.Resolve<IPoliticaTransversalCategoriaServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _politicaTransversalCategoriaController = new PoliticaTransversalCategoriaController(PoliticaTransversalCategoriaServicios, AutorizacionUtilizades);
            _politicaTransversalCategoriaController.ControllerContext.Request = new HttpRequestMessage();

            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAccion", "FFE61CB1-DF37-4C2F-ADA5-0B251A680852");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "472C8A50-DFE7-4C1E-98BC-774EF6991726");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idFormulario", "BAE65BDE-B5E8-4012-93D6-D7896EA65956");
            _politicaTransversalCategoriaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task PoliticaTransversalCategoriaBpinValido()
        {
            var result = (OkNegotiatedContentResult<PoliticaTCategoriasDto>)await _politicaTransversalCategoriaController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Politicas.Count > 0);
        }

        [TestMethod]
        public async Task PoliticaTransversalCategoriaResultadoVacio()
        {
            var resultados = await _politicaTransversalCategoriaController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalCategoriaPreview()
        {
            var result = (OkNegotiatedContentResult<PoliticaTCategoriasDto>)await _politicaTransversalCategoriaController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalCategoriaGuardarTemporal_OK()
        {
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _politicaTransversalCategoriaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAccion", "FFE61CB1-DF37-4C2F-ADA5-0B251A680852");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "472C8A50-DFE7-4C1E-98BC-774EF6991726");

            var politicaTransversalCategoria = new PoliticaTCategoriasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _politicaTransversalCategoriaController.Temporal(politicaTransversalCategoria);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task PoliticaTransversalCategoriaGuardarDefinitivo_OK()
        {
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _politicaTransversalCategoriaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idAccion", "FFE61CB1-DF37-4C2F-ADA5-0B251A680852");
            _politicaTransversalCategoriaController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "472C8A50-DFE7-4C1E-98BC-774EF6991726");

            var politicaTransversalCategoria = new PoliticaTCategoriasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _politicaTransversalCategoriaController.Definitivo(politicaTransversalCategoria);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _politicaTransversalCategoriaController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
