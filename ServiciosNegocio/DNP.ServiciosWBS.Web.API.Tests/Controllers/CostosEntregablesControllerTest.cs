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
    using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;

    using Unity;

    [TestClass]
    public sealed class CostosEntregablesControllerTest : IDisposable
    {
        private ICostosEntregablesServicios CostosEntregablesServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private CostosEntregablesController _costosEntregablesController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            CostosEntregablesServicio = contenedor.Resolve<ICostosEntregablesServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _costosEntregablesController =
                new CostosEntregablesController(CostosEntregablesServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _costosEntregablesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E00485B4-34D9-495A-82CF-7F7F477FD16F");
            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "76A7958B-D20A-48AD-87C9-917D3B388E21");
            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idFormulario", "41EC808E-02FD-4601-9475-81F5C2FEB448");
            _costosEntregablesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task CostosEntregablesBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<CostosEntregablesDto>)await _costosEntregablesController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task CostosEntregablesResultadoVacio()
        {
            var resultados = await _costosEntregablesController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task CostosEntregablesPreviewTest()
        {
            var result = (OkNegotiatedContentResult<CostosEntregablesDto>)await _costosEntregablesController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.vigencias.Count == 1);
        }


        [TestMethod]
        public async Task CostosEntregablesGuardarDefinitivo_OK()
        {
            _costosEntregablesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _costosEntregablesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E00485B4-34D9-495A-82CF-7F7F477FD16F");
            _costosEntregablesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "76A7958B-D20A-48AD-87C9-917D3B388E21");

            var costosActvidadesDto = new CostosEntregablesDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _costosEntregablesController.Definitivo(costosActvidadesDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _costosEntregablesController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
