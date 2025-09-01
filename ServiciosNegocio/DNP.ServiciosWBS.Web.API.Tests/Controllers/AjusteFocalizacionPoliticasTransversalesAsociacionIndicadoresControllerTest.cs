

namespace DNP.ServiciosWBS.Web.API.Tests.Controllers
{
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadoresAjuste;
    using DNP.ServiciosWBS.Servicios.Interfaces;
    using DNP.ServiciosWBS.Web.API.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Unity;
    [TestClass]
    public class AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresControllerTest
    {
        private IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios = contenedor.Resolve<IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController = new AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController(AutorizacionUtilizades, IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios);
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request = new HttpRequestMessage();

            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "D4974BA8-08A6-462E-A140-4ACA76833293");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "E24064A1-AFB8-4379-9916-D056EE018951");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idFormulario", "D4974BA8-08A6-462E-A140-4ACA76833293");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustePoliticaTransversalesAsociacionIndicadoresBpinValido()
        {
            var result = (OkNegotiatedContentResult<PoliticaTIndicadoresAjusteDto>)await _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Politicas.Count > 0);
        }

        [TestMethod]
        public async Task AjustePoliticaTransversalesAsociacionIndicadoresResultadoVacio()
        {
            var resultados = await _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task AjustePoliticaTransversalesAsociacionIndicadoresPreview()
        {
            var result = (OkNegotiatedContentResult<PoliticaTIndicadoresAjusteDto>)await _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task AjustePoliticaTransversalesAsociacionIndicadoresGuardarTemporal_OK()
        {
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "D4974BA8-08A6-462E-A140-4ACA76833293");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "E24064A1-AFB8-4379-9916-D056EE018951");

            var PoliticaTIndicadoresAjusteDto_ = new PoliticaTIndicadoresAjusteDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.Temporal(PoliticaTIndicadoresAjusteDto_);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task AjustePoliticaTransversalesAsociacionIndicadoresGuardarDefinitivo_OK()
        {
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "739D83B2-495E-4C3D-87E9-8492A400698F");
            _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "07EF36DB-D26E-4BB3-8A7E-1D030AD0ACE8");

            var PoliticaTIndicadoresDto_ = new PoliticaTIndicadoresAjusteDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.Definitivo(PoliticaTIndicadoresDto_);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
