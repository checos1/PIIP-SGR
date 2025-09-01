

namespace DNP.ServiciosWBS.Web.API.Tests.Controllers
{
    using API.Controllers;
    using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
    public sealed class AjusteRegionalizacionMetasYRecursosController : IDisposable
    {
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IAjusteRegionalizacionMetasyRecursosServicios AjusteRegionalizacionMetasyRecursosServicios { get; set; }
        private AjustesRegionalizacionMetasyRecursosController _ajustesRegionalizacionMetasyRecursosController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            AjusteRegionalizacionMetasyRecursosServicios = contenedor.Resolve<IAjusteRegionalizacionMetasyRecursosServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _ajustesRegionalizacionMetasyRecursosController =
                new AjustesRegionalizacionMetasyRecursosController(AutorizacionUtilizades, AjusteRegionalizacionMetasyRecursosServicios)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idAccion", "7D7FCC8B-048F-451F-9EE3-CAE2A84D1241");
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1174193F-3F6E-4BCA-8E0E-CCBD59DB113B");
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idFormulario", "1174193F-3F6E-4BCA-8E0E-CCBD59DB113B");
            _ajustesRegionalizacionMetasyRecursosController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustesRegionalizaMetasYRecursosBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<AjusteRegMetasRecursosDto>)await _ajustesRegionalizacionMetasyRecursosController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task AjustesRegionalizarMetasYRecursosResultadoVacio()
        {
            var resultados = await _ajustesRegionalizacionMetasyRecursosController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task AjustesRegionalizaFuentesPreviewTest()
        {
            var result = await _ajustesRegionalizacionMetasyRecursosController.Preview();
            Assert.IsNotNull(result);
            //Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AjustesRegionalizaMetasYRecursosGuardarDefinitivo_OK()
        {
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesRegionalizacionMetasyRecursosController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idAccion", "7D7FCC8B-048F-451F-9EE3-CAE2A84D1241");
            _ajustesRegionalizacionMetasyRecursosController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1174193F-3F6E-4BCA-8E0E-CCBD59DB113B");

            var AjusteRegionalizacionMetasYRecursosDto = new AjusteRegMetasRecursosDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesRegionalizacionMetasyRecursosController.Definitivo(AjusteRegionalizacionMetasYRecursosDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesRegionalizacionMetasyRecursosController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
