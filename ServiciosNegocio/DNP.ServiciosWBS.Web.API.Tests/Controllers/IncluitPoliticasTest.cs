
namespace DNP.ServiciosWBS.Web.API.Tests.Controllers
{
    using API.Controllers;
    using DNP.ServiciosNegocio.Dominio.Dto.AgregarPoliticas;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
    public class IncluitPoliticasTest
    {
        private IIncluirPoliticasServicios IncluirPoliticasServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IncluirPoliticasController _IncluirPoliticasController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            IncluirPoliticasServicios = contenedor.Resolve<IIncluirPoliticasServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _IncluirPoliticasController = new IncluirPoliticasController(AutorizacionUtilizades, IncluirPoliticasServicios);
            _IncluirPoliticasController.ControllerContext.Request = new HttpRequestMessage();

            _IncluirPoliticasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAccion", "739D83B2-495E-4C3D-87E9-8492A400698F");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "07EF36DB-D26E-4BB3-8A7E-1D030AD0ACE8");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idFormulario", "739D83B2-495E-4C3D-87E9-8492A400698F");
            _IncluirPoliticasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task IncluirPoliticasBpinValido()
        {
            var result = (OkNegotiatedContentResult<IncluirPoliticasTDto>)await _IncluirPoliticasController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Politicas.Count > 0);
        }

        [TestMethod]
        public async Task IncluirPoliticasResultadoVacio()
        {
            var resultados = await _IncluirPoliticasController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task IncluirPoliticasPreview()
        {
            var result = (OkNegotiatedContentResult<IncluirPoliticasTDto>)await _IncluirPoliticasController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task IncluirPoliticasGuardarTemporal_OK()
        {
            _IncluirPoliticasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _IncluirPoliticasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAccion", "739D83B2-495E-4C3D-87E9-8492A400698F");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "07EF36DB-D26E-4BB3-8A7E-1D030AD0ACE8");

            var IncluirPoliticasTDto_ = new IncluirPoliticasTDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _IncluirPoliticasController.Temporal(IncluirPoliticasTDto_);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task IncluirPoliticasGuardarDefinitivo_OK()
        {
            _IncluirPoliticasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _IncluirPoliticasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idAccion", "739D83B2-495E-4C3D-87E9-8492A400698F");
            _IncluirPoliticasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "07EF36DB-D26E-4BB3-8A7E-1D030AD0ACE8");

            var IncluirPoliticasTDto_ = new IncluirPoliticasTDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _IncluirPoliticasController.Definitivo(IncluirPoliticasTDto_);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _IncluirPoliticasController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
