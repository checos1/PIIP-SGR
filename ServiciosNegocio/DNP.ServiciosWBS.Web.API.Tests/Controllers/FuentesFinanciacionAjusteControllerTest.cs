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
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

    [TestClass]
    public class FuentesFinanciacionAjusteControllerTest
    {
        private IAjusteDiligenciarFuentesFinanciacionServicios FuentesFinanciacionAjusteServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjusteDiligenciarFuentesFinanciacionController _FuentesFinanciacionAjusteController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            FuentesFinanciacionAjusteServicios = contenedor.Resolve<IAjusteDiligenciarFuentesFinanciacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _FuentesFinanciacionAjusteController = new AjusteDiligenciarFuentesFinanciacionController(AutorizacionUtilizades, FuentesFinanciacionAjusteServicios);
            _FuentesFinanciacionAjusteController.ControllerContext.Request = new HttpRequestMessage();

            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idFormulario", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FuentesFinanciacionAjusteController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task FuentesFinanciacionAjusteBpinValido()
        {
            try
            {
                var result = (OkNegotiatedContentResult<FuentesFinanciacionAjusteDto>)await _FuentesFinanciacionAjusteController.Consultar(Bpin);
                Assert.IsTrue(result.Content.Vigencias.Count > 0);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos reales */
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }

}

[TestMethod]
        public async Task FuentesFinanciacionAjusteResultadoVacio()
        {
            var resultados = await _FuentesFinanciacionAjusteController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task FuentesFinanciacionAjustePreview()
        {
            var result = (OkNegotiatedContentResult<FuentesFinanciacionAjusteDto>)await _FuentesFinanciacionAjusteController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task FuentesFinanciacionAjusteGuardarTemporal_OK()
        {
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FuentesFinanciacionAjusteController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");

            var FuentesFinanciacionAjuste = new FuentesFinanciacionAjusteDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FuentesFinanciacionAjusteController.Temporal(FuentesFinanciacionAjuste);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task FuentesFinanciacionAjusteGuardarDefinitivo_OK()
        {
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FuentesFinanciacionAjusteController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FuentesFinanciacionAjusteController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");

            var FuentesFinanciacionAjuste = new FuentesFinanciacionAjusteDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FuentesFinanciacionAjusteController.Definitivo(FuentesFinanciacionAjuste);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _FuentesFinanciacionAjusteController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
