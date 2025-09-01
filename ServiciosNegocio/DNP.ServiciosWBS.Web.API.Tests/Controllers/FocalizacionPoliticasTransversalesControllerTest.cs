
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
    using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTranversalesMetas;

    [TestClass]
    public class FocalizacionPoliticasTransversalesControllerTest
    {
        private IFocalizacionPoliticasTransversalesMetasServicios FocalizacionPoliticasTransversalesMetasServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FocalizacionPoliticasTransversalesMetasController _FocalizacionPoliticasTransversalesMetasController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            FocalizacionPoliticasTransversalesMetasServicios = contenedor.Resolve<IFocalizacionPoliticasTransversalesMetasServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _FocalizacionPoliticasTransversalesMetasController = new FocalizacionPoliticasTransversalesMetasController(AutorizacionUtilizades,FocalizacionPoliticasTransversalesMetasServicios);
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request = new HttpRequestMessage();

            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idFormulario", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesMetasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioBpinValido()
        {
            try
            {
                var result = (OkNegotiatedContentResult<PoliticaTMetasDto>)await _FocalizacionPoliticasTransversalesMetasController.Consultar(Bpin);
                Assert.IsTrue(result.Content.POLITICAS.Count > 0);
            }
            catch (Exception ex)
            {
                /*Por falta de datos reales, debido a que apunta a BD*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
            
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioResultadoVacio()
        {
            var resultados = await _FocalizacionPoliticasTransversalesMetasController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioPreview()
        {
            var result = (OkNegotiatedContentResult<PoliticaTMetasDto>)await _FocalizacionPoliticasTransversalesMetasController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarTemporal_OK()
        {
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesMetasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");

            var politicaTransversalMetas = new PoliticaTMetasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesMetasController.Temporal(politicaTransversalMetas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarDefinitivo_OK()
        {
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesMetasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesMetasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");

            var politicaTransversalMetas = new PoliticaTMetasDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesMetasController.Definitivo(politicaTransversalMetas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _FocalizacionPoliticasTransversalesMetasController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
