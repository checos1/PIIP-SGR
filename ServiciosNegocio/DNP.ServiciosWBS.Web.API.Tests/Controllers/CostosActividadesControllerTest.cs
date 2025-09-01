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
    using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using Unity;

    [TestClass]
    public sealed class CostosActividadesControllerTest : IDisposable
    {
        private ICostosActividadesServicios CostosActividadesServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private CostosActividadesController _costosActividadesController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            CostosActividadesServicio = contenedor.Resolve<ICostosActividadesServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _costosActividadesController =
                new CostosActividadesController(CostosActividadesServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _costosActividadesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E00485B4-34D9-495A-82CF-7F7F477FD16F");
            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "76A7958B-D20A-48AD-87C9-917D3B388E21");
            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idFormulario", "41EC808E-02FD-4601-9475-81F5C2FEB448");
            _costosActividadesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task CostosActividadesBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<CostosActividadesDto>)await _costosActividadesController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task CostosActividadesResultadoVacio()
        {
            var resultados = await _costosActividadesController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task CostosActividadesPreviewTest()
        {
            var result = (OkNegotiatedContentResult<CostosActividadesDto>)await _costosActividadesController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.vigencias.Count == 1);
        }


        [TestMethod]
        public async Task CostosActividadesGuardarDefinitivo_OK()
        {
            _costosActividadesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _costosActividadesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E00485B4-34D9-495A-82CF-7F7F477FD16F");
            _costosActividadesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "76A7958B-D20A-48AD-87C9-917D3B388E21");

            var costosActvidadesDto = new CostosActividadesDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _costosActividadesController.Definitivo(costosActvidadesDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _costosActividadesController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
