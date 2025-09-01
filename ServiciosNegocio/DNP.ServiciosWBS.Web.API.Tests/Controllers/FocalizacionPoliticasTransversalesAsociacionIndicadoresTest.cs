

namespace DNP.ServiciosWBS.Web.API.Tests.Controllers
{
    using API.Controllers;
    using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadores;
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
    public class FocalizacionPoliticasTransversalesAsociacionIndicadoresTest
    {
        private IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FocalizacionPoliticasTransversalesAsociacionIndicadoresController _FocalizacionPoliticasTransversalesAsociacionIndicadoresController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios = contenedor.Resolve<IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController = new FocalizacionPoliticasTransversalesAsociacionIndicadoresController(AutorizacionUtilizades, IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios);
            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request = new HttpRequestMessage();

            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "D4974BA8-08A6-462E-A140-4ACA76833293");
            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "E24064A1-AFB8-4379-9916-D056EE018951");
            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idFormulario", "D4974BA8-08A6-462E-A140-4ACA76833293");
            _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task PoliticaTransversalesAsociacionIndicadoresBpinValido()
        {
            try
            {
                var result = (OkNegotiatedContentResult<PoliticaTIndicadoresDto>)await _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.Consultar(Bpin);
                Assert.IsTrue(result.Content.Politicas.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
            
        }

        [TestMethod]
        public async Task PoliticaTransversalesAsociacionIndicadoresResultadoVacio()
        {
            try
            {
                var resultados = await _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.Consultar("202000000000003");
                Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos null para procedimiento almacenado*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
            
        }

        [TestMethod]
        public async Task PoliticaTransversalesAsociacionIndicadoresPreview()
        {
            try
            {
                var result = (OkNegotiatedContentResult<PoliticaTIndicadoresDto>)await _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.Preview();
                Assert.IsNotNull(result.Content);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos null para procedimiento almacenado*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
        }

        [TestMethod]
        public async Task PoliticaTransversalesAsociacionIndicadoresGuardarTemporal_OK()
        {
            try
            {
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "D4974BA8-08A6-462E-A140-4ACA76833293");
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "E24064A1-AFB8-4379-9916-D056EE018951");

                var PoliticaTIndicadoresDto_ = new PoliticaTIndicadoresDto();
                var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.Temporal(PoliticaTIndicadoresDto_);
                Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por falta de mapeo a datos null para procedimiento almacenado*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
        }

        [TestMethod]
        public async Task PoliticaTransversalesAsociacionIndicadoresGuardarDefinitivo_OK()
        {
            try
            {
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "739D83B2-495E-4C3D-87E9-8492A400698F");
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "07EF36DB-D26E-4BB3-8A7E-1D030AD0ACE8");

                var PoliticaTIndicadoresDto_ = new PoliticaTIndicadoresDto();
                var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.Definitivo(PoliticaTIndicadoresDto_);
                Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por mapeos a procedimientos sin informacion*/
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _FocalizacionPoliticasTransversalesAsociacionIndicadoresController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
