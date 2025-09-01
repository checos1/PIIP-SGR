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
    public sealed class AjustesRegionalizaFuentesControllerTest : IDisposable
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
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAccion", "7D7FCC8B-048F-451F-9EE3-CAE2A84D1241");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1174193F-3F6E-4BCA-8E0E-CCBD59DB113B");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idFormulario", "1174193F-3F6E-4BCA-8E0E-CCBD59DB113B");
            _regionalizaFuentesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustesRegionalizaFuentesBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<FuenteFinanciacionRegionalizacionDto>)await _regionalizaFuentesController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task AjustesRegionalizaFuentesResultadoVacio()
        {
            var resultados = await _regionalizaFuentesController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task AjustesRegionalizaFuentesPreviewTest()
        {
            var result = (OkNegotiatedContentResult<FuenteFinanciacionRegionalizacionDto>)await _regionalizaFuentesController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.Regionalizacion.Count == 1);
        }


        [TestMethod]
        public async Task AjustesRegionalizaFuentesGuardarDefinitivo_OK()
        {
            _regionalizaFuentesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _regionalizaFuentesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idAccion", "7D7FCC8B-048F-451F-9EE3-CAE2A84D1241");
            _regionalizaFuentesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1174193F-3F6E-4BCA-8E0E-CCBD59DB113B");

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
