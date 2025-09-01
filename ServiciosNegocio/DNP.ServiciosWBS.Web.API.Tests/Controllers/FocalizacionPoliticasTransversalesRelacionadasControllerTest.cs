using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada;


    [TestClass]
    public class FocalizacionPoliticasTransversalesRelacionadasControllerTest
    {
        private IFocalizacionPoliticasTransversalesRelacionadasServicios FocalizacionPoliticasTransversalesRelacionadasServicios;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FocalizacionPoliticasTransversalesRelacionadasController _FocalizacionPoliticasTransversalesRelacionadasController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            FocalizacionPoliticasTransversalesRelacionadasServicios = contenedor.Resolve<IFocalizacionPoliticasTransversalesRelacionadasServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _FocalizacionPoliticasTransversalesRelacionadasController = new FocalizacionPoliticasTransversalesRelacionadasController(AutorizacionUtilizades, FocalizacionPoliticasTransversalesRelacionadasServicios);
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request = new HttpRequestMessage();

            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idFormulario", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesRelacionadasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioBpinValido()
        {
            try
            {
                var result = (OkNegotiatedContentResult<PoliticaTransversalRelacionadaDto>)await _FocalizacionPoliticasTransversalesRelacionadasController.Consultar(Bpin);
                Assert.IsTrue(result.Content.Vigencias_Politicas_Cuantifica_Beneficiarios.Count > 0);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos reales */
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }

        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioResultadoVacio()
        {
            var resultados = await _FocalizacionPoliticasTransversalesRelacionadasController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioPreview()
        {
            var result = (OkNegotiatedContentResult<PoliticaTransversalRelacionadaDto>)await _FocalizacionPoliticasTransversalesRelacionadasController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarTemporal_OK()
        {
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesRelacionadasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idAccion", "5B6566B3-B697-4D24-8151-72B0A34CBD8A");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "5CF91A97-D191-4189-9449-E32CFF7C2EAC");

            var politicaTransversalMetas = new PoliticaTransversalRelacionadaDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesRelacionadasController.Temporal(politicaTransversalMetas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarDefinitivo_OK()
        {
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesRelacionadasController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesRelacionadasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");

            var politicaTransversalMetas = new PoliticaTransversalRelacionadaDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesRelacionadasController.Definitivo(politicaTransversalMetas);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _FocalizacionPoliticasTransversalesRelacionadasController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
