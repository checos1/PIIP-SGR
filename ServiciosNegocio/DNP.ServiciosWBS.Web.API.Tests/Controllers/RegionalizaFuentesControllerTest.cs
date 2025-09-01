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
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using Unity;

    [TestClass]
    public sealed class RegionalizaFuentesControllerTest : IDisposable
    {
        private IRegionalizaFuentesServicio RegionalizaFuentesServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private RegionalizaFuentesController _regionalizaFuentesController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            RegionalizaFuentesServicio = contenedor.Resolve<IRegionalizaFuentesServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _regionalizaFuentesController =
                new RegionalizaFuentesController(RegionalizaFuentesServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _regionalizaFuentesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E00485B4-34D9-495A-82CF-7F7F477FD16F");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "76A7958B-D20A-48AD-87C9-917D3B388E21");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idFormulario", "41EC808E-02FD-4601-9475-81F5C2FEB448");
            _regionalizaFuentesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task RegionalizaFuentesBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<FuenteFinanciacionRegionalizacionDto>)await _regionalizaFuentesController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task RegionalizaFuentesResultadoVacio()
        {
            var resultados = await _regionalizaFuentesController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task RegionalizaFuentesPreviewTest()
        {
            var result = (OkNegotiatedContentResult<FuenteFinanciacionRegionalizacionDto>)await _regionalizaFuentesController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.Regionalizacion.Count == 1);
        }


        [TestMethod]
        public async Task RegionalizaFuentesGuardarDefinitivo_OK()
        {
            _regionalizaFuentesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _regionalizaFuentesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E00485B4-34D9-495A-82CF-7F7F477FD16F");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "76A7958B-D20A-48AD-87C9-917D3B388E21");

            var fuenteFinanciacionRegionalizacionDto = new FuenteFinanciacionRegionalizacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizaFuentesController.Definitivo(fuenteFinanciacionRegionalizacionDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _regionalizaFuentesController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
