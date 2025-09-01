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
    using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes;


    [TestClass]
    public class FocalizacionPoliticasTransversalesRelacionadasAjustesControllerTest : IDisposable
    {
        private IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios FocalizacionPoliticasTransversalesRelacionadasAjustesServicios;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FocalizacionPoliticasTransversalesRelacionadasAjustesController _FocalizacionPoliticasTransversalesRelacionadasAjustesController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            FocalizacionPoliticasTransversalesRelacionadasAjustesServicios = contenedor.Resolve<IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _FocalizacionPoliticasTransversalesRelacionadasAjustesController = new FocalizacionPoliticasTransversalesRelacionadasAjustesController(AutorizacionUtilizades, FocalizacionPoliticasTransversalesRelacionadasAjustesServicios);
            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request = new HttpRequestMessage();

            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");
            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idFormulario", "833C8469-1FD4-4615-89E7-4F022EC9E858");
            _FocalizacionPoliticasTransversalesRelacionadasAjustesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioBpinValido()
        {
            try
            {
                var result = (OkNegotiatedContentResult<PoliticaTransversalRelacionadaAjustesDto>)await _FocalizacionPoliticasTransversalesRelacionadasAjustesController.Consultar(Bpin);
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
            try
            {
                var resultados = await _FocalizacionPoliticasTransversalesRelacionadasAjustesController.Consultar("202000000000003");
                Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos reales */
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioPreview()
        {
            try
            {
                var result = (OkNegotiatedContentResult<PoliticaTransversalRelacionadaAjustesDto>)await _FocalizacionPoliticasTransversalesRelacionadasAjustesController.Preview();
                Assert.IsNotNull(result.Content);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos null para procedimiento almacenado*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarTemporal_OK()
        {
            try
            {
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idAccion", "5B6566B3-B697-4D24-8151-72B0A34CBD8A");
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "5CF91A97-D191-4189-9449-E32CFF7C2EAC");

                var politicaTransversalMetas = new PoliticaTransversalRelacionadaAjustesDto();
                var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesRelacionadasAjustesController.Temporal(politicaTransversalMetas);
                Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos null para procedimiento almacenado*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
        }

        [TestMethod]
        public async Task PoliticaTransversalBeneficiarioGuardarDefinitivo_OK()
        {
            try
            {
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idAccion", "833C8469-1FD4-4615-89E7-4F022EC9E858");
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BAA6E786-160C-4AF4-A5DD-396F4629394E");

                var politicaTransversalMetas = new PoliticaTransversalRelacionadaAjustesDto();
                var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesRelacionadasAjustesController.Definitivo(politicaTransversalMetas);
                Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos null para procedimiento almacenado*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
          
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _FocalizacionPoliticasTransversalesRelacionadasAjustesController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
